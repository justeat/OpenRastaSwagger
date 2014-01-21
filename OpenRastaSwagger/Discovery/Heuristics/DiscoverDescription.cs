using System.Collections.Generic;
using System.Reflection;
using OpenRasta.Configuration.MetaModel;
using OpenRasta.Web;

namespace OpenRastaSwagger.Discovery.Heuristics
{
    public class DiscoverDescription : IDiscoveryHeuristic
    {
        public bool Discover(MethodInfo publicMethod, OperationMetadata methodMetdata)
        {
            methodMetdata.Summary = "Calls " + publicMethod.DeclaringType.Name + "." + publicMethod.Name;

            methodMetdata.Name = "the name";
            methodMetdata.Notes= "the notes";
            methodMetdata.Summary = "the summary";


            return true;
        }
    }

}