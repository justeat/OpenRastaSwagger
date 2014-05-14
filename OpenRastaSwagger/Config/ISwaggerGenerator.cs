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
        List<RequiredHeader> Headers { get; }
        List<Type> ExcludedHandlers { get; }
        IMetaModelRepository MetaModelRepository { get; set; }

        SwaggerGenerator RegisterSwaggerHandler();
        SwaggerGenerator AddRequiredHeader(string name, string suggestedValue);
        SwaggerGenerator GroupByUri();
        SwaggerGenerator GroupByResource();
        void FromConfiguration(IConfigurationSource config);
    }
}