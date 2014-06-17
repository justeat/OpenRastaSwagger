using System;

namespace OpenRastaSwagger.DocumentationSupport
{
    public class RequiredHeaderAttribute : Attribute
    {
        public string Name { get; private set; }
        public Type Type { get; private set; }

        public RequiredHeaderAttribute(string name, Type type)
        {
            Name = name;
            Type = type;
        }
    }
}