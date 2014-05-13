using OpenRasta.Configuration;
using OpenRastaSwagger.Config;
using OpenRastaSwagger.ContractJsonGeneration.Contracts;
using OpenRastaSwagger.ContractJsonGeneration.Handlers;

namespace OpenRastaSwagger.ContractJsonGeneration.Config
{
    public static class SwaggerConfigurationExtensions
    {
        public static void RegisterContract(this SwaggerGenerator cfg, string root = "")
        {
            if (root == "")
            {
                root = cfg.Root;
            }

            ResourceSpace.Has.ResourcesOfType<Contract>()
                .AtUri(string.Format("/{0}/contract", root))
                .HandledBy<ContractHandler>()
                .AsJsonDataContract();
        }
    }
}