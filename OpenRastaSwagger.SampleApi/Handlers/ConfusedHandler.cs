using OpenRasta.Web;
using OpenRastaSwagger.DocumentationSupport;
using OpenRastaSwagger.SampleApi.Resources;

namespace OpenRastaSwagger.SampleApi.Handlers
{
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

        [ResponseTypeIs(typeof(ThirdResource))]
        public OperationResult Get(ComplexRequest req)
        {
            return new OperationResult.OK(new ThirdResource());
        }
    }
}