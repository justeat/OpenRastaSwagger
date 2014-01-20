using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using OpenRasta.Configuration.MetaModel;
using OpenRastaSwagger.Discovery.Heuristics;

namespace OpenRastaSwagger.Discovery
{
    public class HandlerMetadataDiscoverer : IDiscoverHandlerMetadata
    {
        public List<IDiscoveryHeuristic> DiscoveryRules = new List<IDiscoveryHeuristic>
        {
            new DiscoverHttpMethodVerbs(),
        };

        public ResourceMetadata Discover(ResourceModel resource)
        {
            var metadata = new ResourceMetadata {Uris = resource.Uris};

            foreach (var handler in resource.Handlers)
            {
                IndexHandler(handler, metadata);
            }

            return metadata;
        }

        private void IndexHandler(HandlerModel handler, ResourceMetadata metadata)
        {
            var handlerType = handler.Type.StaticType;

            foreach (var uri in metadata.Uris)
            {
                var exclusions = new List<string> { "ToString", "GetType", "GetHashCode", "Equals" };
                foreach (var publicMethod in handlerType.GetMethods().Where(x=>x.IsPublic && !exclusions.Contains(x.Name)))
                {
                    var operation = new OperationMetadata(uri);

                    if (DiscoveryRules.All(x => x.Discover(publicMethod, operation)))
                    {
                        metadata.Add(operation);    
                    }
                    
                }
            }
        }
    }
}