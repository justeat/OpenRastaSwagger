using OpenRastaSwagger.Model.ResourceDetails;

namespace OpenRastaSwagger.Handlers
{
    public class ResourceDetailsHandler
    {
        public ResourceDetails Get(string uri)
        {
            var swag = new Swag();
            return swag.DiscoverSingle(uri);
        }
    }
}