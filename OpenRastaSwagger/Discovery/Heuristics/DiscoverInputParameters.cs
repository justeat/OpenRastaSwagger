using System.Linq;
using System.Reflection;

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

            var paramParser = new UriParameterParser(methodMetdata.Uri.Uri);

            foreach (var param in methodMetdata.InputParameters)
            {
                param.LocationType = InputParameter.LocationTypes.Query;

                if (paramParser.HasParam(param.Name))
                {
                    param.LocationType = paramParser.HasPathParam(param.Name)? InputParameter.LocationTypes.Path : InputParameter.LocationTypes.Query;
                    if (!param.Type.IsPrimitive)
                    {
                        param.Type = typeof(string);
                    }
                }

                if (!TypeMapper.IsTypeSwaggerPrimitive(param.Type))
                {
                    param.LocationType =  InputParameter.LocationTypes.Body;
                }
            }

            return true;
        }
    }
}