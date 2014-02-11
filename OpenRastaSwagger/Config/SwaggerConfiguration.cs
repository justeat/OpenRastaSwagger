using System.Collections.Generic;
using OpenRasta.Configuration;
using OpenRastaSwagger.Grouping;
using OpenRastaSwagger.Handlers;
using OpenRastaSwagger.Model.Contracts;
using OpenRastaSwagger.Model.ResourceDetails;
using OpenRastaSwagger.Model.ResourceListing;

namespace OpenRastaSwagger.Config
{
    public class SwaggerConfiguration
    {
        private static IOperationGrouper _grouper = new OperationGrouperByUri();
        private static readonly List<RequiredHeader> RequiredHeaders = new List<RequiredHeader>();
        
        public static string Root { get; set; }
        public static IOperationGrouper Grouper { get { return _grouper; } }
        public static IEnumerable<RequiredHeader> Headers { get { return RequiredHeaders; } }

        static SwaggerConfiguration()
        {
            Root = "api-docs";
        }

        public static void RegisterSwagger()
        {
            ResourceSpace.Has.ResourcesOfType<ResourceList>()
                .AtUri("/" + Root + "/swagger")
                .HandledBy<SwaggerHandler>()
                .AsJsonDataContract();

            ResourceSpace.Has.ResourcesOfType<ResourceDetails>()
                .AtUri("/" + Root + "/swagger/{groupPath}")
                .HandledBy<SwaggerHandler>()
                .AsJsonDataContract();
        }

        public static void WithHeader(string name, string suggestedValue)
        {
            RequiredHeaders.Add(new RequiredHeader {Name = name, SuggestedValue = suggestedValue});
        }

        public static void GroupByUri()
        {
            _grouper = new OperationGrouperByUri();
        }

        public static void GroupByResource()
        {
            _grouper = new OperationGrouperByResourceType();
        }

        public static void RegisterContract(string root = "")
        {
            if (root == "")
            {
                root = Root;
            }

            ResourceSpace.Has.ResourcesOfType<Contract>()
                .AtUri("/" + root + "/contract")
                .HandledBy<ContractHandler>()
                .AsJsonDataContract();
        }
    }
}