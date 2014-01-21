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
            var key = operation.ReturnType.Name;

            if (operation.ReturnType.IsGenericType)
            {
                key = key.Replace("`1", "-" + operation.ReturnType.GetGenericArguments()[0].Name);
            }
            
            return new OperationGroup
            {
                Name = key,
                Path = "/" + key.ToLower()
            };
        }
    }
}