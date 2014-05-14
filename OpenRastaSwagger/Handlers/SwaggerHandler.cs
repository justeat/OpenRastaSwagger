using System;
using System.Collections.Generic;
using OpenRasta.Collections;
using OpenRastaSwagger.Config;
using OpenRastaSwagger.Model.ResourceDetails;
using OpenRastaSwagger.Model.ResourceListing;

namespace OpenRastaSwagger.Handlers
{
    public class SwaggerHandler
    {
        private readonly ISwaggerDiscoverer _discoverer;

        public SwaggerHandler()
            : this(new SwaggerDiscoverer(), SwaggerGenerator.Configuration.ExcludedHandlers)
        {
        }

        public SwaggerHandler(ISwaggerDiscoverer discoverer, IEnumerable<Type> excludedHandlers)
        {
            _discoverer = discoverer;
            _discoverer.ExcludedHandlers.AddRange(excludedHandlers);
        }

        public ResourceList Get()
        {
            return _discoverer.GetResourceList();
        }

        public ResourceDetails Get(string groupPath)
        {
            return _discoverer.GetResouceDetails(groupPath);
        }
    }
}