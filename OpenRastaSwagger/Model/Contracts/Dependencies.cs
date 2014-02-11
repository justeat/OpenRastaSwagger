namespace OpenRastaSwagger.Model.Contracts
{
    public class Dependencies
    {
        public Dependencies()
        {
            dotNetFramework = new Dotnetframework();
            iis = new Iis();
            memory = new Memory();
            operatingSystem = new Operatingsystem();
            processor = new Processor();
        }

        public Dotnetframework dotNetFramework { get; set; }
        public Iis iis { get; set; }
        public Memory memory { get; set; }
        public Operatingsystem operatingSystem { get; set; }
        public Processor processor { get; set; }
    }
}