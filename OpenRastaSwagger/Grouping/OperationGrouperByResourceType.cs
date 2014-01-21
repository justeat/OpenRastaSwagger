using OpenRasta.Configuration.MetaModel;
using OpenRasta.TypeSystem;
using OpenRasta.TypeSystem.ReflectionBased;
using OpenRastaSwagger.Discovery;

namespace OpenRastaSwagger.Grouping
{
    public class OperationGrouperByResourceType : IOperationGrouper
    {
        public OperationGroup Group(ResourceModel resourceModel, UriModel uriModel, OperationMetadata operation)
        {
            var resourceKey = resourceModel.ResourceKey as ReflectionBasedMember<ITypeBuilder>;
            return new OperationGroup()
            {
                Name = resourceKey.Name,
                Path = "/" + resourceKey.Name.ToLower()
            };
        }
    }
}