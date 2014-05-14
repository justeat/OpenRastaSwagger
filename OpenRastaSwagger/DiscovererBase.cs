using System;
using System.Collections.Generic;
using System.Linq;
using OpenRasta.Configuration.MetaModel;
using OpenRastaSwagger.Config;
using OpenRastaSwagger.Discovery;

namespace OpenRastaSwagger
{
    public abstract class DiscovererBase : IDiscoverer
    {
        public IList<Type> ExcludedHandlers { get; private set; }

        protected DiscovererBase()
        {
            ExcludedHandlers = new List<Type>();
        }

        protected IEnumerable<OperationMetadata> Operations()
        {
            var apiResourceRegistrations = SelectRegistrationsThatArentSwaggerRoutes(SwaggerGenerator.Configuration.MetaModelRepository);

            var discoverer = new ResourceMetadataDiscoverer(SwaggerGenerator.Configuration.Grouper);
            var operations = apiResourceRegistrations.SelectMany(discoverer.Discover);

            return operations;
        }

        private IEnumerable<ResourceModel> SelectRegistrationsThatArentSwaggerRoutes(IMetaModelRepository metaModelRepository)
        {
            var apiResourceRegistrations =
                metaModelRepository.ResourceRegistrations.Where(
                    x => !x.Handlers.Any(h => ExcludedHandlers.Contains(h.Type.StaticType)));
            return apiResourceRegistrations;
        }
    }
}