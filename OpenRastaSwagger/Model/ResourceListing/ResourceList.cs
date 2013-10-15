using System.Collections.Generic;

namespace OpenRastaSwagger.Model.ResourceListing
{
    public class ResourceList
    {
        public string apiVersion { get; set; }
        public string swaggerVersion { get; set; }
        public List<Api> apis { get; set; }

        public ResourceList()
        {
            apis = new List<Api>();
        }
    }
}