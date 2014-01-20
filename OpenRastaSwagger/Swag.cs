using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using OpenRasta.Configuration.MetaModel;
using OpenRasta.DI;
using OpenRasta.TypeSystem;
using OpenRasta.TypeSystem.ReflectionBased;
using OpenRastaSwagger.Discovery;
using OpenRastaSwagger.Handlers;
using OpenRastaSwagger.Model.ResourceDetails;
using OpenRastaSwagger.Model.ResourceListing;
using Api = OpenRastaSwagger.Model.ResourceListing.Api;
using ApiDetails = OpenRastaSwagger.Model.ResourceDetails.Api;

namespace OpenRastaSwagger
{
    public class Swag
    {
        public ResourceList Discover()
        {
            var mmr = DependencyManager.GetService<IMetaModelRepository>();
            return Discover(mmr);
        }

        public ResourceList Discover(IMetaModelRepository metaModelRepository)
        {
            var swaggerSpec = new ResourceList { swaggerVersion = "1.2", apiVersion = Assembly.GetCallingAssembly().GetName().Version.ToString() };
            var apiResourceRegistrations = SelectRegistrationsThatArentSwaggerRoutes(metaModelRepository);

            foreach (var reg in apiResourceRegistrations.Select(x => x.ResourceKey as ReflectionBasedMember<ITypeBuilder>).Distinct())
            {
                swaggerSpec.apis.Add(new Api { description = reg.Name, path = "/" + reg.Name.ToLower() });
            }

            return swaggerSpec;
        }

        private static IEnumerable<ResourceModel> SelectRegistrationsThatArentSwaggerRoutes(IMetaModelRepository metaModelRepository)
        {
            var excludedHandlers = new List<Type>
            {
                typeof (ResourceDetailsHandler),
                typeof (ResourceListingHandler)
            }; 
            
            var apiResourceRegistrations =
                metaModelRepository.ResourceRegistrations.Where(
                    x => x.Handlers.All(h => !excludedHandlers.Contains(h.Type.StaticType)));
            return apiResourceRegistrations;
        }

        public ResourceDetails DiscoverSingle(string path)
        {
            var mmr = DependencyManager.GetService<IMetaModelRepository>();
            return DiscoverSingle(mmr, path);
        }

        public ResourceDetails DiscoverSingle(IMetaModelRepository metaModelRepository, string resourceTypeName)
        {
            var swaggerSpec = new ResourceDetails
            {
                swaggerVersion = "1.2",
                apiVersion = Assembly.GetCallingAssembly().GetName().Version.ToString(),
                apis = new List<ApiDetails>(),
                resourcePath = "/",
                basePath = "/"
            };

            var discoverer = new ResourceMetadataDiscoverer();

            var requestedResource =
                metaModelRepository.ResourceRegistrations.Where(
                    x => x.ResourceKey.ToString().ToUpper().Contains(resourceTypeName.ToUpper()));

            var customTypesForSwagger = new Dictionary<Type, ModelSpec>();

            foreach (var reg in requestedResource)
            {
                var registrationMetadata = discoverer.Discover(reg);
                
                foreach (var operationMetadata in registrationMetadata)
                {
                    var op = new Operation
                    {
                        method = operationMetadata.HttpVerb,
                        nickname = operationMetadata.Name ?? "",
                        notes = operationMetadata.Notes ?? "",
                        type = operationMetadata.ReturnType.Name,
                        summary = operationMetadata.Summary ?? "",
                        parameters = operationMetadata.InputParameters.Select(MapInputParameter).ToList()
                    };

                    RegisterCustomReturnType(customTypesForSwagger, operationMetadata);

                    swaggerSpec.apis.Add(new ApiDetails
                    {
                        description = operationMetadata.Summary,
                        path = operationMetadata.Uri.Uri,
                        operations = new List<Operation> {op}
                    });
                }
            }


            foreach (var item in customTypesForSwagger)
            {
                swaggerSpec.models.Add(item.Key.Name, item.Value);
            }
            return swaggerSpec;
        }

        private Parameter MapInputParameter(InputParameter param)
        {
            return new Parameter()
            {
                paramType = "path",
                type = param.Type.Name,
                name = param.Name
            };
        }


        private static void RegisterCustomReturnType(IDictionary<Type, ModelSpec> customTypesForSwagger, OperationMetadata operationMetadata)
        {
            if (!customTypesForSwagger.ContainsKey(operationMetadata.ReturnType))
            {
                var modelSpec = new ModelSpec { id = operationMetadata.ReturnType.Name };
                foreach (var prop in operationMetadata.ReturnType.GetProperties())
                {
                    modelSpec.properties.Add(prop.Name, new PropertyType {type = prop.PropertyType.Name, description = "Fancy"});
                }
                customTypesForSwagger.Add(operationMetadata.ReturnType, modelSpec);
            }
        }
    }
}
