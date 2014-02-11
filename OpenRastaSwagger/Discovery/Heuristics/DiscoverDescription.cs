using System.Reflection;

namespace OpenRastaSwagger.Discovery.Heuristics
{
    public class DiscoverDescription : IDiscoveryHeuristic
    {
        public bool Discover(MethodInfo publicMethod, OperationMetadata methodMetdata)
        {
            methodMetdata.Summary = "Calls " + publicMethod.DeclaringType.Name + "." + publicMethod.Name;
            methodMetdata.Name = publicMethod.Name;
            
            return true;
        }
    }
}