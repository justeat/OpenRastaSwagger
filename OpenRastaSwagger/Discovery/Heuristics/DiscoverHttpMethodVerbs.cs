using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using OpenRasta.Web;

namespace OpenRastaSwagger.Discovery.Heuristics
{
    public class DiscoverHttpMethodVerbs : IDiscoveryHeuristic
    {
        public bool Discover(MethodInfo publicMethod, OperationMetadata methodMetdata)
        {
            var exclusions = new List<string> {"ToString", "GetType", "GetHashCode", "Equals"};
            var allowedVerbs = new List<string> {"GET", "POST", "PUT", "DELETE", "HEAD", "OPTIONS"};

            if (exclusions.Contains(publicMethod.Name))
            {
                return false;
            }

            var nameUpper = publicMethod.Name.ToUpper();

            var methodAttribute = HttpOperationAttribute.Find(publicMethod);
            if (methodAttribute != null)
            {
                methodMetdata.HttpVerb = methodAttribute.Method;

                /*
                 * If the name configured in OpenRasta doesn't match
                 * the HttpOperation name, then this is the wrong
                 * method to match
                 */
                var uriName = methodMetdata.Uri.Name;
                var operationName = methodAttribute.ForUriName;
                if (!string.IsNullOrWhiteSpace(uriName) && !string.IsNullOrWhiteSpace(operationName) &&
                    !uriName.Equals(operationName, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }

                return true;
            }

            if (allowedVerbs.Contains(nameUpper))
            {
                methodMetdata.HttpVerb = nameUpper;
                return true;
            }

            foreach (var verb in allowedVerbs.Where(nameUpper.StartsWith))
            {
                methodMetdata.HttpVerb = verb;
                return true;
            }

            return false;
        }
    }
}