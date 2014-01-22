using OpenRastaSwagger.Model.Contracts;
using OpenRastaSwagger.Model.ResourceDetails;
using OpenRastaSwagger.Model.ResourceListing;

namespace OpenRastaSwagger.Handlers
{
    public class ContractHandler
    {
        public Contract Get()
        {
            var swag = new ContractDiscoverer();
            return swag.GetContract();
        }
    }
}