using System;
using System.Reflection;
using OpenRasta.TypeSystem;
using OpenRastaSwagger.DocumentationSupport;

namespace OpenRastaSwagger.Discovery.Heuristics
{
    public class DiscoverReturnType : IDiscoveryHeuristic
    {
        public bool Discover(MethodInfo publicMethod, OperationMetadata methodMetdata)
        {
            var responseType = publicMethod.GetCustomAttribute<ResponseTypeIsAttribute>() ??
                               new ResponseTypeIsAttribute(publicMethod.ReturnType);
            methodMetdata.ReturnType = responseType.ResponseType;
            methodMetdata.HandlerType = publicMethod.DeclaringType;

            return IsTypeMatch(methodMetdata.DesiredReturnType, methodMetdata.ReturnType);
        }

        private static bool IsTypeMatch(IMember desiredType, Type returnType)
        {
            return returnType == desiredType.StaticType;
        }
    }
}