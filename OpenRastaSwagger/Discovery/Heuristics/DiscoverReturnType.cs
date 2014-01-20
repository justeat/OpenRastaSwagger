using System.Reflection;

namespace OpenRastaSwagger.Discovery.Heuristics
{
    public class DiscoverReturnType : IDiscoveryHeuristic
    {
        public bool Discover(MethodInfo publicMethod, OperationMetadata methodMetdata)
        {
            methodMetdata.ReturnType = publicMethod.ReturnType;
            return true;
        }
    }
}