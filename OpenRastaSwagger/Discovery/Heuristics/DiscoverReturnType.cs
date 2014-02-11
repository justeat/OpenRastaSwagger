using System.Reflection;
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

            return true;
        }
    }
}