using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using OpenRasta.Collections.Specialized;
using OpenRasta.Configuration;
using OpenRasta.Configuration.MetaModel;
using OpenRasta.DI;
using OpenRasta.TypeSystem;
using OpenRasta.TypeSystem.ReflectionBased;
using OpenRastaSwagger.Discovery;
using OpenRastaSwagger.Handlers;
using OpenRastaSwagger.Model.ResourceDetails;
using OpenRastaSwagger.Model.ResourceListing;
using Api = OpenRastaSwagger.Model.ResourceListing.Api;
using ApiDetails = OpenRastaSwagger.Model.ResourceDetails.Api;

namespace OpenRastaSwagger
{

    public interface IOperationGrouper
    {
        OperationGroup Group(ResourceModel resourceModel, UriModel uriModel, OperationMetadata operation);
    }

    public class OperationGroup
    {
        public string Name { get; set; }
        public string Path { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, this)) return true;

            var other = obj as OperationGroup;
            if (other == null) return false;

            return Name.Equals(other.Name);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }

    public class OperationGrouperByResourceType : IOperationGrouper
    {
        public OperationGroup Group(ResourceModel resourceModel, UriModel uriModel, OperationMetadata operation)
        {
            var resourceKey = resourceModel.ResourceKey as ReflectionBasedMember<ITypeBuilder>;
            return new OperationGroup()
            {
                Name=resourceKey.Name,
                Path="/" + resourceKey.Name.ToLower()
            };
        }
    }

    public class Swag
    {

        private IOperationGrouper _grouper = new OperationGrouperByResourceType();
     
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
            var swaggerSpec = new ResourceList { swaggerVersion = "1.2", apiVersion = Assembly.GetCallingAssembly().GetName().Version.ToString() };
            var groups = Operations().Select(x => x.Group).Distinct().OrderBy(x => x.Name);

            foreach (var group in groups)
            {
                swaggerSpec.apis.Add(new Api { description = group.Name, path = group.Path });
            }

            return swaggerSpec;
        }

        private static IEnumerable<ResourceModel> SelectRegistrationsThatArentSwaggerRoutes(IMetaModelRepository metaModelRepository)
        {
            var apiResourceRegistrations =
                metaModelRepository.ResourceRegistrations.Where(
                    x => x.Handlers.All(h => h.Type.StaticType != typeof (SwaggerHandler)));
            return apiResourceRegistrations;
        }

        public ResourceDetails DiscoverSingle(string groupName)
        {
            var swaggerSpec = new ResourceDetails
            {
                swaggerVersion = "1.2",
                apiVersion = Assembly.GetCallingAssembly().GetName().Version.ToString(),
                apis = new List<ApiDetails>(),
                resourcePath = "/",
                basePath = "/"
            };

            var groupOperations = Operations().Where(x => x.Group.Name.Equals(groupName, StringComparison.InvariantCultureIgnoreCase));

            var customTypesForSwagger = new Dictionary<Type, ModelSpec>();

                
                foreach (var operationMetadata in groupOperations)
                {
                    var op = new Operation
                    {
                        method = operationMetadata.HttpVerb,
                        nickname = operationMetadata.Name ?? "",
                        notes = operationMetadata.Notes ?? "",
                        type = operationMetadata.ReturnType.Name,
                        summary = operationMetadata.Summary ?? "",
                        parameters = new List<Parameter>()
                    };

                    var paramParser = new UriParameterParser(operationMetadata.Uri.Uri);

                    foreach (var param in operationMetadata.InputParameters)
                    {
                        var paramType = paramParser.HasPathParam(param.Name) ? "path" : "query";

                        if (!IsTypeSwaggerPrimitive(param.Type))
                        {
                            paramType = "body";
                        }

                        var swagParam = new Parameter
                        {
                            paramType = paramType,
                            type = param.Type.Name,
                            name = param.Name
                        };

                        op.parameters.Add(swagParam);

                        RegisterCustomType(customTypesForSwagger, param.Type);
                    }

                    RegisterCustomType(customTypesForSwagger, operationMetadata.ReturnType);

                    swaggerSpec.apis.Add(new ApiDetails
                    {
                        description = operationMetadata.Summary,
                        path = paramParser.Path, //operationMetadata.Uri.Uri,
                        operations = new List<Operation> {op}
                    });
                }
            


            foreach (var item in customTypesForSwagger)
            {
                swaggerSpec.models.Add(item.Key.Name, item.Value);
            }
            return swaggerSpec;
        }

        private static bool IsTypeSwaggerPrimitive(Type type)
        {
            return type.IsPrimitive || PrimitiveMappings.ContainsKey(type);
        }

        private static readonly Dictionary<Type, string> PrimitiveMappings = new Dictionary<Type, string>
        {
            {typeof (int), "int32"},
            {typeof (long), "int64"},
            {typeof (float), "float"},
            {typeof (double), "double"},
            {typeof (string), "string"},
            {typeof (byte), "byte"},
            {typeof (bool), "boolean"},
            {typeof (DateTime), "date-time"},
        };

        private static void RegisterCustomType(IDictionary<Type, ModelSpec> customTypesForSwagger, Type returnType)
        {
            if (customTypesForSwagger.ContainsKey(returnType))
            {
                return;
            }

            var modelSpec = new ModelSpec { id = returnType.Name };

            foreach (var prop in returnType.GetProperties())
            {
                var name = PrimitiveMappings.ContainsKey(prop.PropertyType)
                    ? PrimitiveMappings[prop.PropertyType]
                    : prop.PropertyType.Name;

                modelSpec.properties.Add(prop.Name, new PropertyType { type = name, description = "" });

                if (!IsTypeSwaggerPrimitive(prop.PropertyType))
                {
                    RegisterCustomType(customTypesForSwagger, prop.PropertyType);
                }
            }

            customTypesForSwagger.Add(returnType, modelSpec);
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
