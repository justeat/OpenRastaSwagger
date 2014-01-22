using System.Reflection;
using OpenRastaSwagger.Config;
using OpenRastaSwagger.Grouping;
using OpenRastaSwagger.Model.Contracts;
using OpenRastaSwagger.Model.ResourceDetails;
using OpenRastaSwagger.Model.ResourceListing;
using Api = OpenRastaSwagger.Model.ResourceListing.Api;
using Operation = OpenRastaSwagger.Model.ResourceDetails.Operation;
using Parameter = OpenRastaSwagger.Model.ResourceDetails.Parameter;

namespace OpenRastaSwagger
{
    public class ContractDiscoverer : DiscovererBase
    {

        public Contract GetContract()
        {
            var contract = new Contract()
            {
                api = "sample",
                description = "sample API",
                version =  Assembly.GetCallingAssembly().GetName().Version.ToString()
            };

            foreach (var header in SwaggerConfiguration.Headers)
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
                    status = "ProductionReady",
                    
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
     
    }
}