using System;
using System.Collections.Generic;

namespace OpenRastaSwagger
{
    public interface IDiscoverer
    {
        IList<Type> ExcludedHandlers { get; }
    }
}