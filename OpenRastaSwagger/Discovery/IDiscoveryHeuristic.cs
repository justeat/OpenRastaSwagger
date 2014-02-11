using System.Reflection;

namespace OpenRastaSwagger.Discovery
{
    public interface IDiscoveryHeuristic
    {
        bool Discover(MethodInfo method, OperationMetadata methodMetdata);
    }
}