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
        public ISwaggerDiscoverer Discoverer { get; set; }
        public IEnumerable<Type> ExcludedHandlers { get; set; }

        public SwaggerHandler()
        {
            Discoverer = new SwaggerDiscoverer();
            Discoverer.ExcludedHandlers.AddRange(SwaggerGenerator.Configuration.ExcludedHandlers);
        }

        public ResourceList Get()
        {
            return Discoverer.GetResourceList();
        }

        public ResourceDetails Get(string groupPath)
        {
            return Discoverer.GetResouceDetails(groupPath);
        }
    }
}