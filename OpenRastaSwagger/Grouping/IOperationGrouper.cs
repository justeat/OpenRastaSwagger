using OpenRasta.Configuration.MetaModel;
using OpenRastaSwagger.Discovery;

namespace OpenRastaSwagger.Grouping
{
    public interface IOperationGrouper
    {
        OperationGroup Group(ResourceModel resourceModel, UriModel uriModel, OperationMetadata operation);
    }
}