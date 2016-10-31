using System.Linq;
using System.Reflection;
using OpenRastaSwagger.DocumentationSupport;

namespace OpenRastaSwagger.Discovery.Heuristics
{
    public class DiscoverInputParameters : IDiscoveryHeuristic
    {
        public bool Discover(MethodInfo publicMethod, OperationMetadata methodMetdata)
        {
            methodMetdata.InputParameters = publicMethod.GetParameters().Select(param => new InputParameter
            {
                Name = param.Name,
                Type = param.ParameterType

            }).ToList();

            /* 
             * If the OpenRasta configured parameters don't match the method
             * then this isn't the correct method to match
             */
            var methodParameters = methodMetdata.InputParameters.Select(q => q.Name.ToLowerInvariant()).ToArray();
            var uriParameters = methodMetdata.UriParser.Parameters.Select(q => q.ToLowerInvariant()).ToArray();
            if (uriParameters.Except(methodParameters).Any())
            {
                return false;
            }

            foreach (var param in methodMetdata.InputParameters)
            {
                param.LocationType = InputParameter.LocationTypes.Query;

                if (methodMetdata.UriParser.HasParam(param.Name))
                {
                    param.LocationType = methodMetdata.UriParser.HasPathParam(param.Name)
                        ? InputParameter.LocationTypes.Path
                        : InputParameter.LocationTypes.Query;

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

            var requiredHeaders = publicMethod.GetCustomAttributes<RequestHeaderAttribute>();
            foreach (var header in requiredHeaders)
            {
                methodMetdata.InputParameters.Add(new InputParameter
                {
                    Name = header.Name,
                    Type = header.Type, 
                    LocationType = InputParameter.LocationTypes.Header, 
                    IsRequired = header.IsRequired
                });
            }

            return true;
        }
    }
}