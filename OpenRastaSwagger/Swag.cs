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
using OpenRastaSwagger.Model;
using OpenRastaSwagger.Model.Contracts;
using OpenRastaSwagger.Model.ResourceDetails;
using OpenRastaSwagger.Model.ResourceListing;
using Api = OpenRastaSwagger.Model.ResourceListing.Api;
using ApiDetails = OpenRastaSwagger.Model.ResourceDetails.Api;
using Operation = OpenRastaSwagger.Model.ResourceDetails.Operation;
using Parameter = OpenRastaSwagger.Model.ResourceDetails.Parameter;

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
                    type = mappedReturnType.Type,
                    items= mappedReturnType.Items,
                    summary = operationMetadata.Summary ?? "",
                    parameters = new List<Parameter>(),
                    responseMessages = new List<Responsemessage>()
                };

                foreach (var header in RequiredHeaders)
                {
                    op.parameters.Add(new Parameter
                    {
                        paramType = "header",
                        name = header.Name,
                        required = true,
                        description = header.SuggestedValue,
                        type = "string",
                        minimum = 1,
                        maximum = 1
                    });
                }
                
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

        public Contract DiscoverContract()
        {
            var contract = new Contract()
            {
                api = "sample",
                description = "sample API",
                version =  Assembly.GetCallingAssembly().GetName().Version.ToString()
            };

             foreach (var header in RequiredHeaders)
                {
                   contract.commonRequestHeaders.Add(header.Name, new HttpHeader
                    {
                        type =  "string",
                        required = true,
                        description = header.SuggestedValue,
                    });
                }
            var opId = 0;
            foreach (var operationMetadata in Operations())
            {
                var typeMapper = new TypeMapper();

                var mappedReturnType = typeMapper.Register(operationMetadata.ReturnType);

                var op = new Model.Contracts.Operation
                {
                    method = operationMetadata.HttpVerb,
                    urlFormat = operationMetadata.Uri.Uri,
                    description = operationMetadata.Summary,
                    
                    returns = new Returns()
                    {
                        description =operationMetadata.Notes,
                        schema = new Schema()
                        {
                            type = operationMetadata.ReturnType.FriendlyName()
                        }
                    }
                };

   

                foreach (var param in operationMetadata.InputParameters)
                {
                    op.parameters.Add(param.Name, new Model.Contracts.Parameter()
                    {
                        description = param.Type.FriendlyName()
                    });
                }

                op.maxResponseTime.percentiles.Add("_90.0", new Time() { milliseconds = 100 });
                op.maxResponseTime.percentiles.Add("_99.0", new Time() { milliseconds = 125 });
                op.maxResponseTime.percentiles.Add("_99.9", new Time() { milliseconds = 150 });

                contract.operations.Add(string.Format("{0}{1}", operationMetadata.HandlerType.FriendlyName()+operationMetadata.Name, opId), op);

                opId++;
            }

            return contract;
        }

        public static string Root = "api-docs";

        public static List<RequiredHeader> RequiredHeaders = new List<RequiredHeader>();
        
        public static void RegisterSwagger()
        {
            ResourceSpace.Has.ResourcesOfType<ResourceList>()
                .AtUri("/" + Root + "/swagger")
                .HandledBy<SwaggerHandler>()
                .AsJsonDataContract();

            ResourceSpace.Has.ResourcesOfType<ResourceDetails>()
                .AtUri("/" + Root + "/swagger/{resourceTypeName}")
                .HandledBy<SwaggerHandler>()
                .AsJsonDataContract();
        }

        public static void RegisterContract()
        {
            ResourceSpace.Has.ResourcesOfType<Contract>()
                    .AtUri("/" + Root + "/contract")
                   .HandledBy<ContractHandler>()
                   .AsJsonDataContract();            
        }
    }

    public class RequiredHeader
    {
        public string Name { get; set; }
        public string SuggestedValue { get; set; }
    }
}