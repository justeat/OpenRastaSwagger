﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
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
    public class Swag
    {
        public ResourceList Discover()
        {
            var mmr = DependencyManager.GetService<IMetaModelRepository>();
            return Discover(mmr);
        }

        public ResourceList Discover(IMetaModelRepository metaModelRepository)
        {
            var swaggerSpec = new ResourceList { swaggerVersion = "1.2", apiVersion = Assembly.GetCallingAssembly().GetName().Version.ToString() };
            var apiResourceRegistrations = SelectRegistrationsThatArentSwaggerRoutes(metaModelRepository);

            foreach (var reg in apiResourceRegistrations.Select(x => x.ResourceKey as ReflectionBasedMember<ITypeBuilder>).Distinct())
            {
                swaggerSpec.apis.Add(new Api { description = reg.Name, path = "/" + reg.Name.ToLower() });
            }

            return swaggerSpec;
        }

        private static IEnumerable<ResourceModel> SelectRegistrationsThatArentSwaggerRoutes(IMetaModelRepository metaModelRepository)
        {
            var excludedHandlers = new List<Type>
            {
                typeof (ResourceDetailsHandler),
                typeof (ResourceListingHandler)
            }; 
            
            var apiResourceRegistrations =
                metaModelRepository.ResourceRegistrations.Where(
                    x => x.Handlers.All(h => !excludedHandlers.Contains(h.Type.StaticType)));
            return apiResourceRegistrations;
        }

        public ResourceDetails DiscoverSingle(string resourceTypeName)
        {
            var mmr = DependencyManager.GetService<IMetaModelRepository>();
            return DiscoverSingle(mmr, resourceTypeName);
        }

        public ResourceDetails DiscoverSingle(IMetaModelRepository metaModelRepository, string resourceTypeName)
        {
            var swaggerSpec = new ResourceDetails
            {
                swaggerVersion = "1.2",
                apiVersion = Assembly.GetCallingAssembly().GetName().Version.ToString(),
                apis = new List<ApiDetails>(),
                resourcePath = "/",
                basePath = "/"
            };

            var discoverer = new ResourceMetadataDiscoverer();

            var requestedResource =
                metaModelRepository.ResourceRegistrations.Where(
                    x => x.ResourceKey.ToString().ToUpper().Contains(resourceTypeName.ToUpper()));

            var customTypesForSwagger = new Dictionary<Type, ModelSpec>();

            foreach (var reg in requestedResource)
            {
                var registrationMetadata = discoverer.Discover(reg);
                
                foreach (var operationMetadata in registrationMetadata)
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
                .HandledBy<ResourceListingHandler>()
                .AsJsonDataContract();

            ResourceSpace.Has.ResourcesOfType<ResourceDetails>()
                .AtUri("/" + Root + "/{resourceTypeName}")
                .HandledBy<ResourceDetailsHandler>()
                .AsJsonDataContract();
            
        }

    }
}
