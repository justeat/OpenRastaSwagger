using System;
using System.Collections;
using System.Text.RegularExpressions;
using OpenRasta.Configuration.MetaModel;
using OpenRasta.TypeSystem.ReflectionBased;
using OpenRasta.Web;
using OpenRastaSwagger.Discovery;

namespace OpenRastaSwagger.Grouping
{
    public class OperationGrouperByUri : IOperationGrouper
    {
        private readonly Regex _groupRegex = new Regex(@"\/?(\w+)\/");

        public OperationGroup Group(ResourceModel resourceModel, UriModel uriModel, OperationMetadata operation)
        {
            var group = new OperationGroup
            {
                Name = "everything else",
                Path = "misc"
            };                

            var match=_groupRegex.Match(uriModel.Uri);

            if (match.Success)
            {
                group.Name = match.Groups[1].Value.ToLower();
                group.Path = match.Groups[1].Value;
            }

            return group;
        }

    }
}