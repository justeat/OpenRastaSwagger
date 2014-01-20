using System;
using System.Reflection;
using OpenRasta.Configuration.MetaModel;

namespace OpenRastaSwagger.Discovery
{
    public interface IDiscoveryHeuristic
    {
        void Discover(MethodInfo method, OperationMetadata methodMetdata);
    }
}