using System.Text.RegularExpressions;
using OpenRasta.Configuration.MetaModel;
using OpenRastaSwagger.Discovery;

namespace OpenRastaSwagger.Grouping
{
    public class OperationGrouperByUri : IOperationGrouper
    {
        private readonly Regex _groupRegex = new Regex(@"\/?(\w+)\/");

        public OperationGroup Group(ResourceModel resourceModel, UriModel uriModel, OperationMetadata operation)
        {
            var match = _groupRegex.Match(uriModel.Uri);

            if (!match.Success)
            {
                return new OperationGroup { Name = "everything else", Path = "misc" };
            }

            return new OperationGroup
            {
                Name = match.Groups[1].Value.ToLower(),
                Path =  match.Groups[1].Value
            };
        }

    }
}