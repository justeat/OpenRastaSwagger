using OpenRastaSwagger.SampleApi.Resources;

namespace OpenRastaSwagger.SampleApi.Handlers
{
    public class ComplexHandler
    {
        public ComplexResponse Post(ComplexRequest req)
        {
            return new ComplexResponse() { Request= req};
        }

    }
}