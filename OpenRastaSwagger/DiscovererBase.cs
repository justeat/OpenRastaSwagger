using System;
using System.Collections.Generic;
using System.Linq;
using OpenRasta.Configuration.MetaModel;
using OpenRasta.DI;
using OpenRastaSwagger.Config;
using OpenRastaSwagger.Discovery;
using OpenRastaSwagger.Handlers;

namespace OpenRastaSwagger
{
    public class DiscovererBase
    {
        private static readonly IList<Type> ExcludedHandlers = new Type[] { typeof(SwaggerHandler), typeof(ContractHandler)};


        protected IEnumerable<OperationMetadata> Operations()
        {
            var mmr = DependencyManager.GetService<IMetaModelRepository>();
            var apiResourceRegistrations = SelectRegistrationsThatArentSwaggerRoutes(mmr);

            var discoverer = new ResourceMetadataDiscoverer(SwaggerConfiguration.Grouper);
            var operations = apiResourceRegistrations.SelectMany(discoverer.Discover);

            return operations;
        }


        private static IEnumerable<ResourceModel> SelectRegistrationsThatArentSwaggerRoutes(IMetaModelRepository metaModelRepository)
        {
            var apiResourceRegistrations =
                metaModelRepository.ResourceRegistrations.Where(
                    x => !x.Handlers.Any(h => ExcludedHandlers.Contains(h.Type.StaticType)));
            return apiResourceRegistrations;
        }

    }
}