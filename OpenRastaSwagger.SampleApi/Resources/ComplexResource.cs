namespace OpenRastaSwagger.SampleApi.Resources
{
    public enum SomeTypes { TypeA, TypeB, TypeC }

    public class ComplexResource
    {
        public ComplexRequest Request { get; set; }
        public SomeTypes Types { get; set; }
    }
}