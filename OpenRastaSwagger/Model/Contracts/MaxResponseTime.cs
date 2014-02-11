namespace OpenRastaSwagger.Model.Contracts
{
    public class MaxResponseTime
    {
        public MaxResponseTime()
        {
            percentiles = new JsonDictionary<string, Time>();
            limits = new Limits();
        }


        public Limits limits { get; set; }
        public JsonDictionary<string, Time> percentiles { get; set; }
    }
}