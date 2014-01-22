using OpenRastaSwagger.Model.Contracts;
using OpenRastaSwagger.Model.ResourceDetails;
using OpenRastaSwagger.Model.ResourceListing;

namespace OpenRastaSwagger.Handlers
{
    public class SwaggerHandler
    {
        public ResourceList Get()
        {
            var swag = new Swag();
            return swag.Discover();
        }

        public ResourceDetails Get(string resourceTypeName)
        {
            var swag = new Swag();
            return swag.DiscoverSingle(resourceTypeName);
        }

    }
}

