using System;

namespace OpenRastaSwagger.DocumentationSupport
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class InputHeaderAttribute : Attribute
    {
        public string Name { get; private set; }
        public Type Type { get; private set; }
        public bool IsRequired { get; private set; }

        public InputHeaderAttribute(string name, Type type, bool isRequired = false)
        {
            Name = name;
            Type = type;
            IsRequired = isRequired;
        }
    }
}