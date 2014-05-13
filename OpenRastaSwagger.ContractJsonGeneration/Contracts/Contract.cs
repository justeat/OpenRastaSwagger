namespace OpenRastaSwagger.ContractJsonGeneration.Contracts
{
    public class Contract
    {
        public Contract()
        {
            operations = new JsonDictionary<string, Operation>();
            commonRequestHeaders = new JsonDictionary<string, HttpHeader>();
            commonResponseHeaders = new JsonDictionary<string, HttpHeader>();
            dependencies = new Dependencies();
            performanceMonitoring = new PerformanceMonitoring();
        }

        public string api { get; set; }
        public JsonDictionary<string, HttpHeader> commonRequestHeaders { get; set; }
        public JsonDictionary<string, HttpHeader> commonResponseHeaders { get; set; }
        public Dependencies dependencies { get; set; }
        public PerformanceMonitoring performanceMonitoring { get; set; }
        public string description { get; set; }
        public JsonDictionary<string, Operation> operations { get; set; }
        public string version { get; set; }
    }
}
