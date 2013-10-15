using System.Collections.Generic;

namespace OpenRastaSwagger.Model.ResourceDetails
{
    public class Operation
    {
        public string method { get; set; }
        public string nickname { get; set; }
        public string type { get; set; }
        public List<Parameter> parameters { get; set; }
        public string summary { get; set; }
        public string notes { get; set; }
        public List<Responsemessage> responseMessage { get; set; }

        public Operation()
        {
            parameters = new List<Parameter>();
            responseMessage = new List<Responsemessage>();
        }
    }
}