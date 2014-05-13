using OpenRastaSwagger.Model.ResourceDetails;
using OpenRastaSwagger.Model.ResourceListing;

namespace OpenRastaSwagger.Handlers
{
    public class SwaggerHandler
    {
        private readonly ISwaggerDiscoverer _discoverer;

        public SwaggerHandler() : this(new SwaggerDiscoverer())
        {
        }

        public SwaggerHandler(ISwaggerDiscoverer discoverer)
        {
            _discoverer = discoverer;
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