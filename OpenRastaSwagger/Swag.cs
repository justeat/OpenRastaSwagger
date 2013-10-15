using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using OpenRasta.Configuration.MetaModel;
using OpenRasta.DI;
using OpenRasta.Web;
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

            var handlerName = discoveredResource.Handlers[0].Type.Name;
            var handler = DependencyManager.GetService(discoveredResource.Handlers[0].Type.StaticType);


            var swaggerSpec = new ResourceDetails
            {
                swaggerVersion = "1.2",
                apiVersion = "???",
                apis = new List<ApiDetails>
                {
                    new ApiDetails
                    {
                        description = discoveredUri.Name ?? handlerName,
                        path = "/" + discoveredUri.Uri,
                        operations = new List<Operation>()
                    }
                }
            };

            var exclusions = new List<string> { "ToString", "GetType", "GetHashCode", "Equals" };

            foreach (var publicMethod in handler.GetType().GetMethods().Where(x=>x.IsPublic && !exclusions.Contains(x.Name)))
            {
                var method = "GET";
                var contentType = "";
                var methodAttribute = publicMethod.GetCustomAttributes<HttpOperationAttribute>().FirstOrDefault();
                if (methodAttribute != null)
                {
                    method = methodAttribute.Method;
                    contentType = methodAttribute.ContentType.TopLevelMediaType;
                }
                else if (publicMethod.Name.ToUpper() == "GET")
                {
                    method = "GET";
                }
                
                var op = new Operation
                {
                    method = method,
                    nickname = publicMethod.Name,
                    notes = "notes",
                    type = contentType,
                    summary = "summary",
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

                swaggerSpec.apis[0].operations.Add(op);
            }

            return swaggerSpec;
        }
    }
}
