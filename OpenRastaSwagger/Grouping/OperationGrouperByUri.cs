using System.Text.RegularExpressions;
using OpenRasta.Configuration.MetaModel;
using OpenRastaSwagger.Discovery;

namespace OpenRastaSwagger.Grouping
{
    public class OperationGrouperByUri : IOperationGrouper
    {
        private readonly Regex _groupRegex = new Regex(@"\/?([\w-]+)");

        public OperationGroup Group(ResourceModel resourceModel, UriModel uriModel, OperationMetadata operation)
        {
            var operationCanBeGroupedByUri = _groupRegex.Match(uriModel.Uri);

            if (!operationCanBeGroupedByUri.Success)
            {
                return new OperationGroup { Name = "everything else", Path = "misc" };
            }

            string resourceName = operationCanBeGroupedByUri.Groups[1].Value.ToLower();

            return new OperationGroup
            {
                Name = string.Format("Operations about {0}", resourceName),
                Path = resourceName
            };
        }
    }
}