using System.Collections.Generic;

namespace OpenRastaSwagger.Model.ResourceDetails
{
    public class ModelSpec
    {
        public string id { get; set; }
        public JsonDictionary<string, PropertyType> properties { get; set; }
        public List<string> required { get; set; }

        public ModelSpec()
        {
            properties = new JsonDictionary<string, PropertyType>();
            required = new List<string>();
        }
    }
}