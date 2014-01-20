using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
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
                        parameters = new List<Parameter>()
                    };

                    foreach (var param in operationMetadata.InputParameters)
                    {
                        var swagParam = MapInputParameter(param, operationMetadata);
                        op.parameters.Add(swagParam);

                        RegisterCustomType(customTypesForSwagger, param.Type);
                    }

                    RegisterCustomType(customTypesForSwagger, operationMetadata.ReturnType);

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

        private Parameter MapInputParameter(InputParameter param, OperationMetadata operationMetadata)
        {
            string paramType = "query";
            var indexOfPath = operationMetadata.Uri.Uri.ToLower().IndexOf(param.Name.ToLower());
            
            if (indexOfPath > -1)
            {
                var indexOfQuestion = operationMetadata.Uri.Uri.IndexOf('?');
                paramType = indexOfQuestion == -1 ? "path" : (indexOfQuestion > indexOfPath ? "path" : "query");
            }

            if (!param.Type.IsPrimitive && !_primitiveMappings.ContainsKey(param.Type))
            {
                paramType = "body";
            }

            return new Parameter
            {
                paramType = paramType,
                type = param.Type.Name,
                name = param.Name
            };
        }

        private static Dictionary<Type, string> _primitiveMappings = new Dictionary<Type, string>
        {
            {typeof (int), "int32"},
            {typeof (long), "int64"},
            {typeof (float), "float"},
            {typeof (double), "double"},
            {typeof (string), "string"},
            {typeof (byte), "byte"},
            {typeof (bool), "boolean"},
            {typeof (DateTime), "date-time"},
        };

        private static void RegisterCustomType(IDictionary<Type, ModelSpec> customTypesForSwagger, Type returnType)
        {
            if (customTypesForSwagger.ContainsKey(returnType))
            {
                return;
            }

            var modelSpec = new ModelSpec { id = returnType.Name };

            foreach (var prop in returnType.GetProperties())
            {
                var name = _primitiveMappings.ContainsKey(prop.PropertyType)
                    ? _primitiveMappings[prop.PropertyType]
                    : prop.PropertyType.Name;

                modelSpec.properties.Add(prop.Name, new PropertyType { type = name, description = "Fancy" });
                    
                if (!prop.PropertyType.IsPrimitive && !_primitiveMappings.ContainsKey(prop.PropertyType))
                {
                    RegisterCustomType(customTypesForSwagger, prop.PropertyType);
                }
            }

            customTypesForSwagger.Add(returnType, modelSpec);
        }

    }
}
