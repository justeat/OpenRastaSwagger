using OpenRasta.Configuration;
using OpenRastaSwagger.Config;
using OpenRastaSwagger.ContractJsonGeneration.Config;
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
                SwaggerGenerator.Configuration.WithHeader("X-JE-Feature", "Your-Feature-Name");
                SwaggerGenerator.Configuration.WithHeader("Accept-Charset", "utf-8");

                SwaggerGenerator.Configuration.RegisterSwagger();
                SwaggerGenerator.Configuration.RegisterContract();


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