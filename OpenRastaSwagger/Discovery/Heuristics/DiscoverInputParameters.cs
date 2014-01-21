using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using OpenRasta.Configuration.MetaModel;
using OpenRasta.Web;

namespace OpenRastaSwagger.Discovery.Heuristics
{
    public class DiscoverInputParameters : IDiscoveryHeuristic
    {
        public bool Discover(MethodInfo publicMethod, OperationMetadata methodMetdata)
        {
            methodMetdata.InputParameters = publicMethod.GetParameters().Select(param => new InputParameter()
            {
                Name = param.Name,
                Type = param.ParameterType
            }).ToList();

            return true;
        }
    }

}