using System;
using System.Collections.Generic;
using System.Linq;
using OpenRasta.Configuration;
using OpenRasta.Configuration.MetaModel;
using OpenRasta.DI;
using OpenRastaSwagger.Grouping;
using OpenRastaSwagger.Handlers;
using OpenRastaSwagger.Model.ResourceDetails;
using OpenRastaSwagger.Model.ResourceListing;

namespace OpenRastaSwagger.Config
{
    public class SwaggerGenerator
    {
        public string Root { get; set; }

        public IOperationGrouper Grouper { get; private set; }
        public IEnumerable<RequiredHeader> Headers { get { return _requiredHeaders; } }
        public IDependencyResolver Resolver { get; set; }
        public IList<Type> ExcludedHandlers { get; private set; }

        private readonly List<RequiredHeader> _requiredHeaders;

        private static readonly Lazy<SwaggerGenerator> Singleton = new Lazy<SwaggerGenerator>(() => new SwaggerGenerator());
        public static SwaggerGenerator Configuration { get { return Singleton.Value; } }

        private SwaggerGenerator()
        {
            Root = "api-docs";
            Grouper = new OperationGrouperByUri();
            ExcludedHandlers = new List<Type>();
            _requiredHeaders = new List<RequiredHeader>();
        }

        private IMetaModelRepository _metaModelRepository;

        public IMetaModelRepository MetaModelRepository
        {
            get
            {
                if (_metaModelRepository != null)
                {
                    return _metaModelRepository;
                }
                if (Resolver != null)
                {
                    return Resolver.Resolve<IMetaModelRepository>();
                }

                return DependencyManager.GetService<IMetaModelRepository>();

            }
            set { _metaModelRepository = value; }
        }

        public void FromConfiguration(IConfigurationSource config)
        {
            using (var host = new NullHost(config))
            {
                _metaModelRepository = host.Resolver.Resolve(typeof(IMetaModelRepository)) as IMetaModelRepository;
            }
        }

        public SwaggerGenerator RegisterSwaggerHandler()
        {
            ExcludedHandlers.Add(typeof(SwaggerHandler));

            ResourceSpace.Has.ResourcesOfType<ResourceList>()
                .AtUri(string.Format("/{0}/swagger", Root))
                .HandledBy<SwaggerHandler>()
                .AsJsonDataContract();

            ResourceSpace.Has.ResourcesOfType<ResourceDetails>()
                .AtUri(string.Format("/{0}/swagger/{{groupPath}}", Root))
                .HandledBy<SwaggerHandler>()
                .AsJsonDataContract();

            return this;
        }

        public SwaggerGenerator AddRequiredHeader(string name, string suggestedValue)
        {
            if (_requiredHeaders.Any(x => x.Name == name))
            {
                _requiredHeaders.RemoveAll(x => x.Name == name);
            }

            _requiredHeaders.Add(new RequiredHeader {Name = name, SuggestedValue = suggestedValue});
            return this;
        }

        public SwaggerGenerator GroupByUri()
        {
            Grouper = new OperationGrouperByUri();
            return this;
        }

        public SwaggerGenerator GroupByResource()
        {
            Grouper = new OperationGrouperByResourceType();
            return this;
        }
    }
}