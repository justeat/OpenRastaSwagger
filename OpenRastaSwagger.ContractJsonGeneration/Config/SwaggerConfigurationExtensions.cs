using OpenRasta.Configuration;
using OpenRastaSwagger.Config;
using OpenRastaSwagger.ContractJsonGeneration.Contracts;
using OpenRastaSwagger.ContractJsonGeneration.Handlers;

namespace OpenRastaSwagger.ContractJsonGeneration.Config
{
    public static class SwaggerConfigurationExtensions
    {
        public static SwaggerGenerator RegisterContractJsonHandler(this SwaggerGenerator cfg, string root = "")
        {
            if (root == "")
            {
                root = cfg.Root;
            }

            cfg.ExcludedHandlers.Add(typeof(ContractHandler));

            ResourceSpace.Has.ResourcesOfType<Contract>()
                .AtUri(string.Format("/{0}/contract", root))
                .HandledBy<ContractHandler>()
                .AsJsonDataContract();

            return cfg;
        }
    }
}