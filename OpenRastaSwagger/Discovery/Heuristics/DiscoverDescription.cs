using System.ComponentModel;
using System.Reflection;

namespace OpenRastaSwagger.Discovery.Heuristics
{
    public class DiscoverDescription : IDiscoveryHeuristic
    {
        public bool Discover(MethodInfo publicMethod, OperationMetadata methodMetdata)
        {
            var descriptionAttribute = publicMethod.GetCustomAttribute<DescriptionAttribute>();

            methodMetdata.Summary = descriptionAttribute == null 
                ? "" 
                : descriptionAttribute.Description;
            
            return true;
        }
    }
}