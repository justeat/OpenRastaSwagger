namespace OpenRastaSwagger.ContractJsonGeneration.Contracts
{
    public class Schema
    {
        public Schema()
        {
            properties = new JsonDictionary<string, Parameter>();
        }

        public JsonDictionary<string, Parameter> properties { get; set; }
        public string type { get; set; }
    }
}