namespace OpenRastaSwagger.Model.ResourceDetails
{
    public class Parameter
    {
        public string paramType { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string type { get; set; }
        public string format { get; set; }
        public bool required { get; set; }
        public int minimum { get; set; }
        public int maximum { get; set; }
    }
}