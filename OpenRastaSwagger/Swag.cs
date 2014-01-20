using System;
using System.Collections.Generic;
using System.Linq;
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
            var swaggerSpec = new ResourceList {swaggerVersion = "1.2", apiVersion = "???"};
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

        public ResourceDetails DiscoverSingle(IMetaModelRepository metaModelRepository, string path)
        {
            UriModel discoveredUri = null;

            var swaggerSpec = new ResourceDetails
            {
                swaggerVersion = "1.2",
                apiVersion = "???",
                apis = new List<ApiDetails>()
            };

            var discoverer = new ResourceMetadataDiscoverer();

            foreach (var reg in metaModelRepository.ResourceRegistrations)
            {
                var registrationMetadata = discoverer.Discover(reg);
                
                var apiDetails = new ApiDetails
                {
                    description = "",
                    path = "/" + path,
                    operations = new List<Operation>()
                };

                foreach (var operationMetadata in registrationMetadata)
                {
                    var op = new Operation
                    {
                        method = operationMetadata.HttpVerb,
                        nickname = operationMetadata.Name ?? "",
                        notes = operationMetadata.Notes ?? "",
                        type = operationMetadata.ContentType ?? "",
                        summary = operationMetadata.Summary ?? "",
                        parameters = new List<Parameter>
                        {
                            new Parameter
                            {
                                description = "desc",
                                format = "format",
                                maximum = 1,
                                minimum = 1,
                                name = "name",
                                paramType = "paramType",
                                required = false,
                                type = "type"
                            }
                        }
                    };

                    apiDetails.operations.Add(op);
                    swaggerSpec.apis.Add(apiDetails);
                }
            }

            return swaggerSpec;
        }
    }
}
