using System.Runtime.Serialization;

namespace OpenRastaSwagger.Model.ResourceDetails
{
    public class PropertyType
    {
        public string type { get; set; }
        public string description { get; set; }
        public string format { get; set; }
        public Items items { get; set; }
    }

    [DataContract]
    public class Items
    {
        [DataMember(Name = "$ref")]
        public string Ref { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }

    }


}