using System;

namespace OpenRastaSwagger.Discovery
{
    public class InputParameter
    {
        public string Name { get; set; }
        public Type Type { get; set; }
        public LocationTypes LocationType { get; set; }
        public bool IsRequired { get; set; }

        public enum LocationTypes { Query, Path, Body, Header }
    }
}