using System.Collections.Generic;
using System.Reflection;
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
                foreach (var publicMethod in handlerType.GetMethods())
                {
                    IndexHttpOperation(metadata, uri, publicMethod);
                }
            }
        }

        private void IndexHttpOperation(ICollection<OperationMetadata> metadata, UriModel uri, MethodInfo publicMethod)
        {
            var operation = new OperationMetadata(uri);

            foreach (var heuristic in DiscoveryRules)
            {
                heuristic.Discover(publicMethod, operation);
            }

            metadata.Add(operation);
        }
    }
}