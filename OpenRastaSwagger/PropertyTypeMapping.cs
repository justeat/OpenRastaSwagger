namespace OpenRastaSwagger
{
    public class PropertyTypeMapping
    {
        public string Type { get; set; }
        public string Format { get; set; }

        public PropertyTypeMapping(string type, string format="")
        {
            Type = type;
            Format = format;
        }

    }
}