using System;
using System.Collections.Generic;
using OpenRasta.Configuration.MetaModel;
using OpenRasta.TypeSystem;
using OpenRastaSwagger.Grouping;

namespace OpenRastaSwagger.Discovery
{
    public class OperationMetadata
    {
        public UriModel Uri { get; set; }
        public string HttpVerb { get; set; }
        public string Nickname { get; set; }
        public string Notes { get; set; }
        public string ContentType { get; set; }
        public string Summary { get; set; }

        public Type ReturnType { get; set; }
        public List<InputParameter> InputParameters { get; set; }

        public OperationGroup Group { get; set; }
        public List<ResponseCode> ResponseCodes { get; set; }

        public UriParameterParser UriParser { get; private set; }

        public Type HandlerType { get; set; }

        public IMember DesiredReturnType { get; private set; }

        public OperationMetadata(UriModel uri, IMember returnType)
        {
            Uri = uri;
            DesiredReturnType = returnType;
            ResponseCodes = new List<ResponseCode>();
            UriParser=new UriParameterParser(Uri.Uri);
        }
    }
}