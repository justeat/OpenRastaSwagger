using System;
using System.Collections.Generic;
using OpenRasta.Configuration.MetaModel;
using OpenRastaSwagger.DocumentationSupport;
using OpenRastaSwagger.Grouping;

namespace OpenRastaSwagger.Discovery
{
    public class OperationMetadata
    {
        public UriModel Uri { get; set; }
        public string HttpVerb { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public string ContentType { get; set; }
        public string Summary { get; set; }

        public Type ReturnType { get; set; }
        public List<InputParameter> InputParameters { get; set; }

        public OperationGroup Group { get; set; }
        public List<ResponseCode> ResponseCodes { get; set; }

        public OperationMetadata(UriModel uri)
        {
            Uri = uri;
            ResponseCodes = new List<ResponseCode>();
        }
    }
}