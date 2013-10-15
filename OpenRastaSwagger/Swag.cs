using System.Collections.Generic;
using System.Linq;
using OpenRasta.Configuration.MetaModel;
using OpenRasta.DI;
using OpenRasta.TypeSystem.ReflectionBased;
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
            ResourceModel discoveredResource = null;

            foreach (var reg in metaModelRepository.ResourceRegistrations)
            {
                foreach (var uri in reg.Uris.Where(x=>x.Uri == "/" + path))
                {
                    discoveredResource = reg;
                    discoveredUri = uri;
                }
            }
            
            var swaggerSpec = new ResourceDetails
            {
                swaggerVersion = "1.2",
                apiVersion = "???",
                apis = new List<ApiDetails>
                {
                    new ApiDetails
                    {
                        description = discoveredUri.Name,
                        path = "/" + discoveredUri.Uri,
                        operations = new List<Operation>
                        {
                            new Operation
                            {
                                method = discoveredResource.ResourceKey.ConvertToString(),
                                nickname = "nick",
                                notes = "notes",
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
                            }
                        }
                    }
                }
            };

            return swaggerSpec;
        }
    }
}
