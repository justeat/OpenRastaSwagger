using OpenRasta.Configuration.MetaModel;

namespace OpenRastaSwagger.Discovery
{
    public interface IDiscoverHandlerMetadata
    {
        ResourceMetadata Discover(ResourceModel resource);
    }
}