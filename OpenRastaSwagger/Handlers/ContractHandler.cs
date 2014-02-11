using OpenRastaSwagger.Model.Contracts;

namespace OpenRastaSwagger.Handlers
{
    public class ContractHandler
    {
        public Contract Get()
        {
            return new ContractDiscoverer().GetContract();
        }
    }
}