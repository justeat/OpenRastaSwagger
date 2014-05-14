using OpenRasta.Configuration;
using OpenRastaSwagger.Config;
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
                SwaggerGenerator.Configuration
                                .AddRequiredHeader("X-JE-Feature", "Your-Feature-Name")
                                .AddRequiredHeader("Accept-Charset", "utf-8")
                                .RegisterSwaggerHandler();

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

                ResourceSpace.Has.ResourcesOfType<string>()
                    .AtUri("/doNotDiscoverMe")
                    .HandledBy<AbstractHandlerThatCannotBeCreated>()
                    .AsJsonDataContract();
            }
        }
    }
}