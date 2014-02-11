using System.Reflection;

namespace OpenRastaSwagger.Discovery.Heuristics
{
    public class DiscoverDescription : IDiscoveryHeuristic
    {
        public bool Discover(MethodInfo publicMethod, OperationMetadata methodMetdata)
        {
            var declaringTypeName = "";
            if (publicMethod.DeclaringType != null)
            {
                declaringTypeName = publicMethod.DeclaringType.Name + ".";
            }

            methodMetdata.Summary = "Calls " + declaringTypeName + publicMethod.Name;
            methodMetdata.Name = publicMethod.Name;
            
            return true;
        }
    }
}