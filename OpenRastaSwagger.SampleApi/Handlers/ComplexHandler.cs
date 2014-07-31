using OpenRastaSwagger.SampleApi.Resources;

namespace OpenRastaSwagger.SampleApi.Handlers
{
    public class ComplexHandler
    {
        public ComplexResource Post(ComplexRequest req)
        {
            return new ComplexResource {Request = req};
        }
    }

    public class ConfusedHandler
    {
        public ComplexResource Post(ComplexRequest req)
        {
            return new ComplexResource { Request = req };
        }

        public SimpleResource Put(ComplexRequest req)
        {
            return new SimpleResource();
        }
    }
}