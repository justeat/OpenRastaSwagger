using OpenRastaSwagger.SampleApi.Resources;

namespace OpenRastaSwagger.SampleApi.Handlers
{
    public class BounceHandler
    {
        public Bounce Get(string message)
        {
            return new Bounce{ Message = message };
        }

    }
}