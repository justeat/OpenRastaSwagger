using OpenRasta.Web;
using OpenRastaSwagger.SampleApi.Resources;

namespace OpenRastaSwagger.SampleApi.Handlers
{
    public class MultiMethodHandler
    {
        [HttpOperation(HttpMethod.GET)]
        public Home Get()
        {
            return new Home { Title = "Welcome home." };
        }

        [HttpOperation(HttpMethod.POST)]
        public OperationResult Post()
        {
            var home = new Home { Title = "Welcome home." };
            
            return new OperationResult.OK(home);
        }
    }
}