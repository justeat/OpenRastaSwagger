using OpenRastaSwagger.ContractJsonGeneration.Contracts;

namespace OpenRastaSwagger.ContractJsonGeneration.Handlers
{
    public class ContractHandler
    {
        public Contract Get()
        {
            return new ContractDiscoverer().GetContract();
        }
    }
}