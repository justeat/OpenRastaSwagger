using System.Collections.Generic;

namespace OpenRastaSwagger.Model.ResourceDetails
{
    public class ResourceDetails
    {
        public string apiVersion { get; set; }
        public string swaggerVersion { get; set; }
        public string basePath { get; set; }
        public string resourcePath { get; set; }
        public List<Api> apis { get; set; }

        public ResourceDetails()
        {
            apis = new List<Api>();
        }
    }
}