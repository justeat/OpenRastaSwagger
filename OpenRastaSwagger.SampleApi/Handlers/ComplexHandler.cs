using OpenRasta.Web;
using OpenRastaSwagger.DocumentationSupport;
using OpenRastaSwagger.SampleApi.Resources;

namespace OpenRastaSwagger.SampleApi.Handlers
{
    public class ComplexHandler
    {
        public ComplexResource Post(ComplexRequest req)
        {
            return new ComplexResource() { Request= req};
        }
    }

    public class HandlerWithAttributes
    {
        [ResponseTypeIs(typeof(string))]
        [PossibleResponseCodeAttribute(201, "I created this awesome thing.")]
        [PossibleResponseCodeAttribute(500, "Ouch - this is rendered when blah blah blah")]
        public OperationResult Get(bool createSomething)
        {
            if (createSomething)
            {
                return new OperationResult.Created();
            }
            
            return new OperationResult.InternalServerError{Description = "Ouch"};
        }
    }
}