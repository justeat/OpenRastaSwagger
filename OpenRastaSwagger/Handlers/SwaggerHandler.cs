using OpenRastaSwagger.Model.ResourceDetails;
using OpenRastaSwagger.Model.ResourceListing;

namespace OpenRastaSwagger.Handlers
{
    public class SwaggerHandler
    {
        public ResourceList Get()
        {
            return new SwaggerDiscoverer().GetResourceList();
        }

        public ResourceDetails Get(string groupPath)
        {
            return new SwaggerDiscoverer().GetResouceDetails(groupPath);
        }
    }
}

