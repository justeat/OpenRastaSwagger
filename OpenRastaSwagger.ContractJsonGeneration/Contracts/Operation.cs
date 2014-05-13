namespace OpenRastaSwagger.ContractJsonGeneration.Contracts
{
    public class Operation
    {
        public Operation()
        {
            parameters = new JsonDictionary<string, Parameter>();
            maxResponseTime = new MaxResponseTime();
        }
        public string description { get; set; }
        public string method { get; set; }
        public JsonDictionary<string, Parameter> parameters { get; set; }
        public Returns returns { get; set; }
        public string status { get; set; }
        public string urlFormat { get; set; }
        public MaxResponseTime maxResponseTime { get; set; }
    }
}