using OpenRastaSwagger.SampleApi.Resources;

namespace OpenRastaSwagger.SampleApi.Handlers
{
    public class HomeHandler
    {
        public Home Get()
        {
            return new Home { Title = "Welcome home." };
        }

        public Home NotGet()
        {
            return new Home { Title = "Welcome home." };
        }

    }
}