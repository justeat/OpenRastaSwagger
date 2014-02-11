using OpenRastaSwagger.SampleApi.Resources;

namespace OpenRastaSwagger.SampleApi.Handlers
{
    public class SimpleHandler
    {
        public SimpleResource Get(string message)
        {
            return new SimpleResource{ Name = message };
        }
    }
}