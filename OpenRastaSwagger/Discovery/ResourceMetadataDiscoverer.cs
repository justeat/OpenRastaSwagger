using System;
using System.Collections.Generic;
using System.Linq;
using OpenRasta.Configuration.MetaModel;
using OpenRasta.TypeSystem;
using OpenRasta.TypeSystem.ReflectionBased;
using OpenRastaSwagger.Discovery.Heuristics;
using OpenRastaSwagger.Grouping;

namespace OpenRastaSwagger.Discovery
{
    public class ResourceMetadataDiscoverer : IDiscoverHandlerMetadata
    {
        private readonly IOperationGrouper _grouper;

        public ResourceMetadataDiscoverer(IOperationGrouper grouper)
        {
            _grouper = grouper;
        }

        public List<IDiscoveryHeuristic> DiscoveryRules = new List<IDiscoveryHeuristic>
        {
            new DiscoverHttpMethodVerbs(),
            new DiscoverDescription(),
            new DiscoverResponseCodes(),
            new DiscoverReturnType(),
            new DiscoverInputParameters(),
            new DiscoverNotes(),
        };

        public ResourceMetadata Discover(ResourceModel resource)
        {
            var metadata = new ResourceMetadata {Uris = resource.Uris};

            var filteredHandlers = resource.Handlers.Where(x => !x.Type.StaticType.IsAbstract);
            foreach (var handler in filteredHandlers)
            {
                IndexHandler(resource, handler, metadata);
            }

            return metadata;
        }

        private void IndexHandler(ResourceModel resource, HandlerModel handler, ResourceMetadata metadata)
        {
            var exclusions = new List<string> { "ToString", "GetType", "GetHashCode", "Equals" };

            foreach (var uri in metadata.Uris)
            {
                var candidateMethods = handler.Type.StaticType.GetMethods()
                    .Where(x => x.IsPublic && !exclusions.Contains(x.Name) && !x.IsSpecialName && IsTypeMatch(resource, x.ReturnType));

                foreach (var publicMethod in candidateMethods)
                {
                    var operation = new OperationMetadata(uri);

                    if (DiscoveryRules.All(x => x.Discover(publicMethod, operation)))
                    {
                        metadata.Add(operation);
                        operation.Group = _grouper.Group(resource, uri, operation);
                    }
                }
            }
        }

        private static bool IsTypeMatch(ResourceModel resource, Type returnType)
        {
            var resourceKey = (IType) resource.ResourceKey;
            var resourceType = resourceKey.StaticType;

            return returnType == resourceType;
        }
    }
}