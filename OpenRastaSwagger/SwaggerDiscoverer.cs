using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using OpenRastaSwagger.Config;
using OpenRastaSwagger.Grouping;
using OpenRastaSwagger.Model.ResourceDetails;
using OpenRastaSwagger.Model.ResourceListing;
using Api = OpenRastaSwagger.Model.ResourceListing.Api;
using ApiDetails = OpenRastaSwagger.Model.ResourceDetails.Api;

namespace OpenRastaSwagger
{
    public class SwaggerDiscoverer : DiscovererBase, ISwaggerDiscoverer
    {
        public ResourceList GetResourceList()
        {
            return GetResourceList(group => "/" + group.Path);
        }

        public ResourceList GetResourceList(Func<OperationGroup, string> groupingOperation)
        {
            var swaggerSpec = new ResourceList
            {
                swaggerVersion = "1.2",
                apiVersion = Assembly.GetCallingAssembly().GetName().Version.ToString()
            };
            var groups = Operations().Select(x => x.Group).Distinct().OrderBy(x => x.Name);

            foreach (var group in groups)
            {
                swaggerSpec.apis.Add(new Api {description = group.Name, path = groupingOperation(group)});
            }

            return swaggerSpec;
        }

        public ResourceDetails GetResouceDetails(string groupPath)
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
                var mappedReturnType = typeMapper.Register(operationMetadata.ReturnType);

                var op = new Operation
                {
                    method = operationMetadata.HttpVerb,
                    nickname = operationMetadata.Nickname ?? "",
                    notes = operationMetadata.Notes ?? "",
                    type = mappedReturnType.Type,
                    items = mappedReturnType.Items,
                    summary = operationMetadata.Summary ?? "",
                    parameters = new List<Parameter>(),
                    responseMessages = new List<Responsemessage>()
                };

                foreach (var header in SwaggerGenerator.Configuration.Headers)
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
    }
}