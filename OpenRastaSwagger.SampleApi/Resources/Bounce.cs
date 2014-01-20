using OpenRasta.Binding;

namespace OpenRastaSwagger.SampleApi.Resources
{
    public class Bounce
    {
        public string Message{ get; set; }
    }

    public class ComplexResponse
    {
        public ComplexRequest Request { get; set; }
    }

    public class ComplexRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }
}