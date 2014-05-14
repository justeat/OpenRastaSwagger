using System;
using System.Collections.Generic;
using OpenRasta.Configuration;
using OpenRasta.Configuration.MetaModel;
using OpenRasta.DI;
using OpenRastaSwagger.Grouping;

namespace OpenRastaSwagger.Config
{
    public interface ISwaggerGenerator
    {
        string Root { get; set; }

        IDependencyResolver Resolver { get; set; }
        IOperationGrouper Grouper { get; }
        IList<RequiredHeader> Headers { get; }
        IList<Type> ExcludedHandlers { get; }
        IMetaModelRepository MetaModelRepository { get; set; }

        ISwaggerGenerator RegisterSwaggerHandler();
        ISwaggerGenerator AddRequiredHeader(string name, string suggestedValue);
        ISwaggerGenerator GroupByUri();
        ISwaggerGenerator GroupByResource();

        void FromConfiguration(IConfigurationSource config);
    }
}