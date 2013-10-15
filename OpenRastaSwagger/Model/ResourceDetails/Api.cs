using System.Collections.Generic;

namespace OpenRastaSwagger.Model.ResourceDetails
{
    public class Api
    {
        public string path { get; set; }
        public string description { get; set; }
        public List<Operation> operations { get; set; }

        public Api()
        {
            operations = new List<Operation>();
        }
    }
}