using OpenRastaSwagger.Model.ResourceListing;

namespace OpenRastaSwagger.Handlers
{
    public class ResourceListingHandler
    {
        public ResourceList Get()
        {
            var swag = new Swag();
            return swag.Discover();
        }
    }
}

