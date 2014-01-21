using System;

namespace OpenRastaSwagger.DocumentationSupport
{
    public class ResponseTypeIsAttribute : Attribute
    {
        public Type ResponseType { get; set; }

        public ResponseTypeIsAttribute(Type responseType)
        {
            ResponseType = responseType;
        }
    }
}