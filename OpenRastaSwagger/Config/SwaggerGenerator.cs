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
        private static readonly Lazy<SwaggerGenerator> LazyInstance = new Lazy<SwaggerGenerator>(()=> new SwaggerGenerator()); 
        public static SwaggerGenerator Configuration
        {
            get { return LazyInstance.Value; }
        }

        private IOperationGrouper _grouper = new OperationGrouperByUri();
        private readonly List<RequiredHeader> _requiredHeaders = new List<RequiredHeader>();
        
        public string Root { get; set; }
        public IOperationGrouper Grouper { get { return _grouper; } }
        public IEnumerable<RequiredHeader> Headers { get { return _requiredHeaders; } }

        private SwaggerGenerator()
        {
            Root = "api-docs";
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

        public IDependencyResolver Resolver { get; set; }

        public void RegisterSwaggerHandler()
        {
            ResourceSpace.Has.ResourcesOfType<ResourceList>()
                .AtUri(string.Format("/{0}/swagger", Root))
                .HandledBy<SwaggerHandler>()
                .AsJsonDataContract();

            ResourceSpace.Has.ResourcesOfType<ResourceDetails>()
                .AtUri(string.Format("/{0}/swagger/{{groupPath}}", Root))
                .HandledBy<SwaggerHandler>()
                .AsJsonDataContract();
        }

        public void AddRequiredHeader(string name, string suggestedValue)
        {
            if (_requiredHeaders.Any(x => x.Name == name))
            {
                _requiredHeaders.RemoveAll(x => x.Name == name);
            }

            _requiredHeaders.Add(new RequiredHeader {Name = name, SuggestedValue = suggestedValue});
        }

        public void GroupByUri()
        {
            _grouper = new OperationGrouperByUri();
        }

        public void GroupByResource()
        {
            _grouper = new OperationGrouperByResourceType();
        }
    }
}