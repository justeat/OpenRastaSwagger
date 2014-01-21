using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using OpenRastaSwagger.DocumentationSupport;

namespace OpenRastaSwagger.Discovery.Heuristics
{
    public class DiscoverDescription : IDiscoveryHeuristic
    {
        public bool Discover(MethodInfo publicMethod, OperationMetadata methodMetdata)
        {
            methodMetdata.Summary = "Calls " + publicMethod.DeclaringType.Name + "." + publicMethod.Name;
            methodMetdata.Name = publicMethod.Name;
            
            var notes = publicMethod.GetCustomAttribute<NotesAttribute>() ?? new NotesAttribute("");
            var possibleResponseCodes = publicMethod.GetCustomAttributes<PossibleResponseCodeAttribute>() ?? new List<PossibleResponseCodeAttribute>();
            var responseType = publicMethod.GetCustomAttribute<ResponseTypeIsAttribute>() ?? new ResponseTypeIsAttribute(publicMethod.ReturnType);

            methodMetdata.Notes = notes.Notes;
            methodMetdata.Notes += "Returns " + publicMethod.ReturnType.FullName;

            foreach (var code in possibleResponseCodes)
            {
                methodMetdata.ResponseCodes.Add(new ResponseCode(code.StatusCode, code.Description));
            }

            return true;
        }
    }

}