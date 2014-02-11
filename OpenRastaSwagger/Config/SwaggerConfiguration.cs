using System.Collections.Generic;
using OpenRasta.Configuration;
using OpenRastaSwagger.Grouping;
using OpenRastaSwagger.Handlers;
using OpenRastaSwagger.Model.Contracts;
using OpenRastaSwagger.Model.ResourceDetails;
using OpenRastaSwagger.Model.ResourceListing;

namespace OpenRastaSwagger.Config
{
    public static class SwaggerConfiguration
    {
        private static string _root = "api-docs";

        public static string Root
        {
            get { return _root; }
            set { _root = value; }
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

        private static IOperationGrouper _grouper=new OperationGrouperByUri();
        public static IOperationGrouper Grouper { get { return _grouper; } }

        public static void GroupByUri()
        {
            _grouper = new OperationGrouperByUri();
        }

        public static void GroupByResource()
        {
            _grouper = new OperationGrouperByResourceType();
        }

        public static void RegisterContract()
        {
            ResourceSpace.Has.ResourcesOfType<Contract>()
                .AtUri("/" + Root + "/contract")
                .HandledBy<ContractHandler>()
                .AsJsonDataContract();
        }

        public static void WithHeader(string name, string suggestedValue)
        {
            RequiredHeaders.Add(new RequiredHeader {Name = name, SuggestedValue = suggestedValue});
        }

        public static IEnumerable<RequiredHeader> Headers { get { return RequiredHeaders; } }
        private static readonly List<RequiredHeader> RequiredHeaders = new List<RequiredHeader>();
    }
}