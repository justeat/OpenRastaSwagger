using System;

namespace OpenRastaSwagger.DocumentationSupport
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class PossibleResponseCodeAttribute : Attribute
    {
        public int StatusCode { get; set; }
        public string Description { get; set; }

        public PossibleResponseCodeAttribute(int statusCode, string description)
        {
            StatusCode = statusCode;
            Description = description;
        }
    }
}