using OpenRastaSwagger.SampleApi.Resources;

namespace OpenRastaSwagger.SampleApi.Handlers
{
    public class ParameterizedHandler
    {
        public SimpleResource Get(string messageFormat, string parameter)
        {
            return new SimpleResource { Name = string.Format(messageFormat, parameter) };
        }
    }
}