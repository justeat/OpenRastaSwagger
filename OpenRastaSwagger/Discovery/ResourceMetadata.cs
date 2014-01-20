using System.Collections.Generic;
using System.Reflection;
using OpenRasta.Configuration.MetaModel;
using OpenRasta.Web;

namespace OpenRastaSwagger.Discovery
{
    public class ResourceMetadata : List<OperationMetadata>
    {
        public IList<UriModel> Uris { get; set; }
    }
}