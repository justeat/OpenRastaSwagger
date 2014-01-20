using System;
using System.Reflection;
using OpenRasta.Configuration.MetaModel;

namespace OpenRastaSwagger.Discovery
{
    public interface IDiscoveryHeuristic
    {
        bool Discover(MethodInfo method, OperationMetadata methodMetdata);
    }
}