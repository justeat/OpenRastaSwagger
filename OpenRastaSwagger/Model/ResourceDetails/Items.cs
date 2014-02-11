using System.Runtime.Serialization;

namespace OpenRastaSwagger.Model.ResourceDetails
{
    [DataContract]
    public class Items
    {
        [DataMember(Name = "$ref")]
        public string Ref { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }
    }
}