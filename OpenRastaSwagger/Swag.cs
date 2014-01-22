using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using OpenRasta.Configuration;
using OpenRasta.Configuration.MetaModel;
using OpenRasta.DI;
using OpenRastaSwagger.Discovery;
using OpenRastaSwagger.Grouping;
using OpenRastaSwagger.Handlers;
using OpenRastaSwagger.Model.ResourceDetails;
using OpenRastaSwagger.Model.ResourceListing;
using Api = OpenRastaSwagger.Model.ResourceListing.Api;
using ApiDetails = OpenRastaSwagger.Model.ResourceDetails.Api;

namespace OpenRastaSwagger
{
    public class Swag
    {
        //private readonly IOperationGrouper _grouper = new OperationGrouperByResourceType();
        private readonly IOperationGrouper _grouper = new OperationGrouperByUri();

        private IEnumerable<OperationMetadata> Operations()
        {
            var mmr = DependencyManager.GetService<IMetaModelRepository>();
            var apiResourceRegistrations = SelectRegistrationsThatArentSwaggerRoutes(mmr);

            var discoverer = new ResourceMetadataDiscoverer(_grouper);
            var operations = apiResourceRegistrations.SelectMany(discoverer.Discover);

            return operations;
        }

        public ResourceList Discover()
        {
            var swaggerSpec = new ResourceList
            {
                swaggerVersion = "1.2",
                apiVersion = Assembly.GetCallingAssembly().GetName().Version.ToString()
            };
            var groups = Operations().Select(x => x.Group).Distinct().OrderBy(x => x.Name);

            foreach (var group in groups)
            {
                swaggerSpec.apis.Add(new Api {description = group.Name, path = "/"+group.Path});
            }

            return swaggerSpec;
        }

        private static IEnumerable<ResourceModel> SelectRegistrationsThatArentSwaggerRoutes(
            IMetaModelRepository metaModelRepository)
        {
            var apiResourceRegistrations =
                metaModelRepository.ResourceRegistrations.Where(
                    x => x.Handlers.All(h => h.Type.StaticType != typeof (SwaggerHandler)));
            return apiResourceRegistrations;
        }

        public ResourceDetails DiscoverSingle(string groupPath)
        {
            var swaggerSpec = new ResourceDetails
            {
                swaggerVersion = "1.2",
                apiVersion = Assembly.GetCallingAssembly().GetName().Version.ToString(),
                apis = new List<ApiDetails>(),
                resourcePath = "/",
                basePath = "/"
            };

            var groupOperations =
                Operations().Where(x => x.Group.Path.Equals(groupPath, StringComparison.InvariantCultureIgnoreCase));
            
            var typeMapper = new TypeMapper();

            foreach (var operationMetadata in groupOperations)
            {
                var mappedReturnType=typeMapper.Register(operationMetadata.ReturnType);
              
                var op = new Operation
                {
                    method = operationMetadata.HttpVerb,
                    nickname = operationMetadata.Name ?? "",
                    notes = operationMetadata.Notes ?? "",
                    type = mappedReturnType.type,
                    items= mappedReturnType.items,
                    summary = operationMetadata.Summary ?? "",
                    parameters = new List<Parameter>(),
                    responseMessages = new List<Responsemessage>()
                };
                
                foreach (var param in operationMetadata.InputParameters)
                {
                    var swagParam = typeMapper.Map(param);
                    swagParam.paramType = param.LocationType.ToString().ToLower();
                    op.parameters.Add(swagParam);
                }

                foreach (var code in operationMetadata.ResponseCodes)
                {
                    op.responseMessages.Add(new Responsemessage {code = code.StatusCode, message = code.Description});
                }

                swaggerSpec.apis.Add(new ApiDetails
                {
                    description = operationMetadata.Summary,
                    path = operationMetadata.UriParser.Path, 
                    operations = new List<Operation> {op}
                });
            }

            foreach (var item in typeMapper.Models)
            {
                swaggerSpec.models.Add(item.id, item);
            }

            return swaggerSpec;
        }

        public static string Root = "api-docs";


        public static void Configure()
        {
            ResourceSpace.Has.ResourcesOfType<ResourceList>()
                .AtUri("/" + Root)
                .HandledBy<SwaggerHandler>()
                .AsJsonDataContract();

            ResourceSpace.Has.ResourcesOfType<ResourceDetails>()
                .AtUri("/" + Root + "/{resourceTypeName}")
                .HandledBy<SwaggerHandler>()
                .AsJsonDataContract();
        }
    }
}