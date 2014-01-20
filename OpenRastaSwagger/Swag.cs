using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using OpenRasta.Configuration.MetaModel;
using OpenRasta.DI;
using OpenRasta.Web;
using OpenRastaSwagger.Discovery;
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

            foreach (var uri in metaModelRepository.ResourceRegistrations.SelectMany(reg => reg.Uris))
            {
                swaggerSpec.apis.Add(new Api{description = uri.Name, path = uri.Uri});
            }

            return swaggerSpec;
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
                        nickname = operationMetadata.Name,
                        notes = operationMetadata.Notes,
                        type = operationMetadata.ContentType,
                        summary = operationMetadata.Summary,
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
