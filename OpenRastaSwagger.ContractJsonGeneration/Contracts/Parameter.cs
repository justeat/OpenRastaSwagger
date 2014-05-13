namespace OpenRastaSwagger.ContractJsonGeneration.Contracts
{
    public class Parameter
    {
        public string description { get; set; }
        public bool required { get; set; }
        public string type { get; set; }
        public string format { get; set; }
        public Schema schema { get; set; }
    }
}