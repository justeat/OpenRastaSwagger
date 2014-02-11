using System.Collections.Generic;
using OpenRasta.Configuration.MetaModel;

namespace OpenRastaSwagger.Discovery
{
    public class ResourceMetadata : List<OperationMetadata>
    {
        public IList<UriModel> Uris { get; set; }
    }
}