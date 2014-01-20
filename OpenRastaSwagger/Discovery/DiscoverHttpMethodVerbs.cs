using System.Collections.Generic;
using System.Reflection;
using OpenRasta.Web;

namespace OpenRastaSwagger.Discovery
{
    public class DiscoverHttpMethodVerbs : IDiscoveryHeuristic
    {
        public void Discover(MethodInfo publicMethod, OperationMetadata methodMetdata)
        {
            var exclusions = new List<string> {"ToString", "GetType", "GetHashCode", "Equals"};

            if (exclusions.Contains(publicMethod.Name))
            {
                return;
            }
            
            var methodAttribute = HttpOperationAttribute.Find(publicMethod);
            if (methodAttribute != null)
            {
                methodMetdata.HttpVerb = methodAttribute.Method;
            }
            else if (publicMethod.Name.ToUpper() == "GET")
            {
                 methodMetdata.HttpVerb = "GET";
            }
        }
    }
}