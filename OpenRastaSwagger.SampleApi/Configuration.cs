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
                Swag.Configure();

                ResourceSpace.Has.ResourcesOfType<SimpleResource>()
                    .AtUri("/simple/{message}")
                    .And.AtUri("/simple/?message={message}&pageNumber={pageNumber}")
                    .HandledBy<SimpleHandler>()
                    .AsJsonDataContract();

                ResourceSpace.Has.ResourcesOfType<ComplexResource>()
                    .AtUri("/complex")
                    .HandledBy<ComplexHandler>()
                    .AsJsonDataContract();
                    
                ResourceSpace.Has.ResourcesOfType<ComplexResource>()
                    .AtUri("/withAttributes")
                    .HandledBy<HandlerWithAttributes>()
                    .AsJsonDataContract();
                    

            }
        }
    }
}


