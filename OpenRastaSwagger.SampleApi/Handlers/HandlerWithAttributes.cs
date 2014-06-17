using System.ComponentModel;
using OpenRasta.Web;
using OpenRastaSwagger.DocumentationSupport;

namespace OpenRastaSwagger.SampleApi.Handlers
{
    public class HandlerWithAttributes
    {
        [ResponseTypeIs(typeof(string))]
        [Description("The description for attribute handler")]
        [PossibleResponseCode(201, "I created this awesome thing.")]
        [PossibleResponseCode(500, "Ouch - this is rendered when blah blah blah")]
        [RequiredHeader("Some header", typeof(int))]
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