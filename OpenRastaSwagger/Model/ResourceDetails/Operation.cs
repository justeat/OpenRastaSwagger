using System.Collections.Generic;

namespace OpenRastaSwagger.Model.ResourceDetails
{
    public class Operation
    {
        public string method { get; set; }
        public string nickname { get; set; }
        public string type { get; set; }
        public Items items { get; set; }
        public List<Parameter> parameters { get; set; }
        public string summary { get; set; }
        public string notes { get; set; }
        public List<Responsemessage> responseMessages { get; set; }

        public Operation()
        {
            parameters = new List<Parameter>();
            responseMessages = new List<Responsemessage>();
        }
    }
}