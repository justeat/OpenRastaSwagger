using System.Reflection;
using OpenRastaSwagger.DocumentationSupport;

namespace OpenRastaSwagger.Discovery.Heuristics
{
    public class DiscoverNotes : IDiscoveryHeuristic
    {
        public bool Discover(MethodInfo publicMethod, OperationMetadata methodMetadata)
        {
            var notes = publicMethod.GetCustomAttribute<NotesAttribute>() ?? new NotesAttribute("");
            methodMetadata.Notes = notes.Notes;
            methodMetadata.Notes += "Uri template " + methodMetadata.Uri.Uri;

            return true;
        }
    }
}