using System.Reflection;
using OpenRastaSwagger.DocumentationSupport;

namespace OpenRastaSwagger.Discovery.Heuristics
{
    public class DiscoverNotes : IDiscoveryHeuristic
    {
        public bool Discover(MethodInfo publicMethod, OperationMetadata methodMetadata)
        {
            var attribute = publicMethod.GetCustomAttribute<NotesAttribute>() ?? new NotesAttribute("");

            if (string.IsNullOrEmpty(attribute.Notes))
            {
                methodMetadata.Notes += "Uri template " + methodMetadata.Uri.Uri;
            }
            else
            {
                methodMetadata.Notes = attribute.Notes;
            }

            return true;
        }
    }
}