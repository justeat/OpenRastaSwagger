using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenRasta.TypeSystem.ReflectionBased;

namespace OpenRastaSwagger
{
    public static class TypeExtensions
    {
        public static string FriendlyName(this Type type)
        {
            if (type.Implements<IEnumerable>() && type != typeof(string))
            {
                Type collectionType = type.GetElementType();

                if (type.IsGenericType)
                {
                    collectionType = type.GetGenericArguments()[0];
                }

                return "IEnumerable[" + collectionType.Namespace + "." + collectionType.Name + "]";
            }

            if (type.IsGenericType)
            {
                var genericArgument = type.GetGenericArguments()[0];
                return type.Name + "[" + genericArgument.Namespace + "." + genericArgument.Name + "]";
            }

            return type.Name;
        }
    }
}
