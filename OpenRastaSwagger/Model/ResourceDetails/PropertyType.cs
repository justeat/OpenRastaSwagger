using System.Collections.Generic;
using System.Dynamic;
using System.Runtime.Serialization;

namespace OpenRastaSwagger.Model.ResourceDetails
{
    [DataContract]
    public class PropertyType
    {
        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "format")]
        public string Format { get; set; }

        [DataMember(Name = "items")]
        public Items Items { get; set; }

        [DataMember(Name = "enum")]
        public string[] Enum { get; set; }
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