using OpenRastaSwagger.Model.Contracts;
using OpenRastaSwagger.Model.ResourceDetails;
using OpenRastaSwagger.Model.ResourceListing;

namespace OpenRastaSwagger.Handlers
{
    public class SwaggerHandler
    {
        public ResourceList Get()
        {
            var swag = new SwaggerDiscoverer();
            return swag.GetResourceList();
        }

        public ResourceDetails Get(string groupPath)
        {
            var swag = new SwaggerDiscoverer();
            return swag.GetResouceDetails(groupPath);
        }

    }
}

