using System;
using System.Collections.Generic;
using OpenRasta.Collections;
using OpenRastaSwagger.Config;
using OpenRastaSwagger.ContractJsonGeneration.Contracts;

namespace OpenRastaSwagger.ContractJsonGeneration.Handlers
{
    public class ContractHandler
    {
        private readonly ContractDiscoverer _discoverer;

        public ContractHandler()
            : this(new ContractDiscoverer(), SwaggerGenerator.Configuration.ExcludedHandlers)
        {
        }

        public ContractHandler(ContractDiscoverer discoverer, IEnumerable<Type> excludedHandlers)
        {
            _discoverer = discoverer;
            _discoverer.ExcludedHandlers.AddRange(excludedHandlers);
        }

        public Contract Get()
        {
            return _discoverer.GetContract();
        }
    }
}