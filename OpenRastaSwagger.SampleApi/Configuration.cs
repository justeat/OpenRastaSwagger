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

                ResourceSpace.Has.ResourcesOfType<Home>()
                    .AtUri("/home")
                    .And.AtUri("/home-again")
                    .HandledBy<HomeHandler>()
                    .AsJsonDataContract();

                ResourceSpace.Has.ResourcesOfType<Home>()
                    .AtUri("/home2")
                    .And.AtUri("/home-again2")
                    .HandledBy<HomeHandler2>()
                    .AsJsonDataContract();

                ResourceSpace.Has.ResourcesOfType<Home>()
                    .AtUri("/getAndPost")
                    .HandledBy<MultiMethodHandler>()
                    .AsJsonDataContract();
            }
        }
    }
}


