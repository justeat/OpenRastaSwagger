using OpenRasta.Codecs;
using OpenRasta.Configuration;
using OpenRastaSwagger.Handlers;
using OpenRastaSwagger.Model.ResourceDetails;
using OpenRastaSwagger.Model.ResourceListing;
using OpenRastaSwagger.SampleApi.Handlers;
using OpenRastaSwagger.SampleApi.Resources;

namespace OpenRastaSwagger.SampleApi
{
    public class Configuration : IConfigurationSource
    {
        public void Configure()
        {
            using (OpenRastaConfiguration.Manual)
            {
                ResourceSpace.Has.ResourcesOfType<ResourceList>()
                    .AtUri("/api-docs")
                    .HandledBy<ResourceListingHandler>()
                    .AsJsonDataContract();

                ResourceSpace.Has.ResourcesOfType<ResourceDetails>()
                    .AtUri("/api-docs/{uri}")
                    .HandledBy<ResourceDetailsHandler>()
                    .AsJsonDataContract();

                ResourceSpace.Has.ResourcesOfType<SimpleResource>()
                    .AtUri("/simple/{message}")
                    .HandledBy<SimpleHandler>()
                    .AsJsonDataContract();

                ResourceSpace.Has.ResourcesOfType<ComplexResource>()
                    .AtUri("/complex")
                    .HandledBy<ComplexHandler>()
                    .AsJsonDataContract();
                    

            }
        }
    }
}


