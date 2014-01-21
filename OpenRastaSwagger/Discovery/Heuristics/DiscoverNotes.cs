using System.Reflection;
using OpenRastaSwagger.DocumentationSupport;

namespace OpenRastaSwagger.Discovery.Heuristics
{
    public class DiscoverNotes : IDiscoveryHeuristic
    {
        public bool Discover(MethodInfo publicMethod, OperationMetadata methodMetdata)
        {
            var notes = publicMethod.GetCustomAttribute<NotesAttribute>() ?? new NotesAttribute("");
            methodMetdata.Notes = notes.Notes;
            methodMetdata.Notes += "Returns " + publicMethod.ReturnType.FullName;

            return true;
        }
    }
}