using System.Collections.Generic;
using System.Reflection;
using OpenRasta.Web;

namespace OpenRastaSwagger.Discovery.Heuristics
{
    public class DiscoverHttpMethodVerbs : IDiscoveryHeuristic
    {
        public void Discover(MethodInfo publicMethod, OperationMetadata methodMetdata)
        {
            var exclusions = new List<string> {"ToString", "GetType", "GetHashCode", "Equals"};

            var allowedVerbs= new List<string> { "GET", "POST", "PUT", "DELETE", "HEAD", "OPTIONS" };

            if (exclusions.Contains(publicMethod.Name))
            {
                return;
            }

            var nameUpper = publicMethod.Name.ToUpper();

            var methodAttribute = HttpOperationAttribute.Find(publicMethod);
            if (methodAttribute != null)
            {
                methodMetdata.HttpVerb = methodAttribute.Method;
                return;
            }

            if (allowedVerbs.Contains(nameUpper))
            {
                methodMetdata.HttpVerb = nameUpper;
                return;
            }

            foreach (var verb in allowedVerbs)
            {
                if (nameUpper.StartsWith(verb))
                {
                    methodMetdata.HttpVerb = verb;
                    return;
                }
            }
        }
    }
}