using System.ComponentModel;
using System.Reflection;

namespace OpenRastaSwagger.Discovery.Heuristics
{
    public class DiscoverDescription : IDiscoveryHeuristic
    {
        public bool Discover(MethodInfo publicMethod, OperationMetadata methodMetdata)
        {
            var descriptionAttribute = publicMethod.GetCustomAttribute<DescriptionAttribute>();

            methodMetdata.Nickname = publicMethod.Name;
            methodMetdata.Summary = descriptionAttribute == null 
                ? GetMethodName(publicMethod)
                : descriptionAttribute.Description;
            
            return true;
        }

        static string GetMethodName(MethodInfo publicMethod)
        {
            string typeName = publicMethod.DeclaringType == null 
                ? ""
                : publicMethod.DeclaringType.Name;
            
            return string.Format("{0}.{1}", typeName, publicMethod.Name);
        }
    }
}