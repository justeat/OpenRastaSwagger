using System.Collections.Generic;
using System.Reflection;
using OpenRastaSwagger.DocumentationSupport;

namespace OpenRastaSwagger.Discovery.Heuristics
{
    public class DiscoverResponseCodes : IDiscoveryHeuristic
    {
        public bool Discover(MethodInfo publicMethod, OperationMetadata methodMetdata)
        {
            var possibleResponseCodes = publicMethod.GetCustomAttributes<PossibleResponseCodeAttribute>() ??
                                        new List<PossibleResponseCodeAttribute>();

            foreach (var code in possibleResponseCodes)
            {
                methodMetdata.ResponseCodes.Add(new ResponseCode(code.StatusCode, code.Description));
            }

            return true;
        }
    }
}