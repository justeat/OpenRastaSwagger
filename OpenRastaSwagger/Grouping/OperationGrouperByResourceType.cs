using System;
using System.Collections;
using OpenRasta.Configuration.MetaModel;
using OpenRasta.TypeSystem.ReflectionBased;
using OpenRasta.Web;
using OpenRastaSwagger.Discovery;

namespace OpenRastaSwagger.Grouping
{
    public class OperationGrouperByResourceType : IOperationGrouper
    {
        public OperationGroup Group(ResourceModel resourceModel, UriModel uriModel, OperationMetadata operation)
        {
            if (IsUnknownReturnType(operation.ReturnType))
            {
                return new OperationGroup { Name = "Unknown", Path = "unknown" };                
            }

            if ((operation.ReturnType != typeof(string)) 
                && operation.ReturnType.Implements<IEnumerable>())
            {
                var collectionType = operation.ReturnType.GetElementType();

                if (operation.ReturnType.IsGenericType)
                {
                    collectionType = operation.ReturnType.GetGenericArguments()[0];
                }

                return new OperationGroup
                {
                    Name = "Collection of " + collectionType.Name,
                    Path = collectionType.Name.ToLower()+"[]"
                };
            }

            return new OperationGroup
            {
                Name = operation.ReturnType.Name,
                Path = operation.ReturnType.Name.ToLower()
            };
        }

        private static bool IsUnknownReturnType(Type type)
        {
            return (type == typeof (OperationResult));
        }
    }
}