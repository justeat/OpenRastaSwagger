using System;
using System.Collections;
using System.Collections.Generic;
using OpenRasta.TypeSystem.ReflectionBased;
using OpenRasta.Web;
using OpenRastaSwagger.Discovery;
using OpenRastaSwagger.Model.ResourceDetails;

namespace OpenRastaSwagger
{
    public class TypeMapper
    {
        private readonly IDictionary<Type, ModelSpec> _models = new Dictionary<Type, ModelSpec>();

        public IEnumerable<ModelSpec> Models
        {
            get { return _models.Values; }
        }

        public Parameter Map(InputParameter param)
        {
            var mapping = Register(param.Type);

            return new Parameter
            {
                type = mapping.Type,
                format = mapping.Format,
                name = param.Name
            };
        }

        public PropertyType Register(Type returnType, int depth = 0)
        {
            returnType = Nullable.GetUnderlyingType(returnType) ?? returnType;

            if (_models.ContainsKey(returnType))
            {
                return new PropertyType()
                {
                    Type = _models[returnType].id,
                    Description = returnType.FriendlyName(),
                    Items = new Items()
                    {
                        Ref = _models[returnType].id
                    }
                };
            }

            if (PrimitiveMappings.ContainsKey(returnType))
            {
                var mapping = PrimitiveMappings[returnType];
                return new PropertyType()
                {
                    Description = returnType.FriendlyName(),
                    Type = mapping.Type,
                    Format = mapping.Format
                };
            }

            if (returnType.IsEnum)
            {
                return new PropertyType
                {
                    Description = returnType.FriendlyName(),
                    Type = "string",
                    Enum = Enum.GetNames(returnType)
                };
            }

            if (returnType.Implements<IEnumerable>())
            {
                var collectionType = returnType.GetElementType();

                if (returnType.IsGenericType)
                {
                    collectionType = returnType.GetGenericArguments()[0];
                }

                var colMapping = Register(collectionType, depth++);

                var isComplex = _models.ContainsKey(collectionType);

                return new PropertyType
                {
                    Description = returnType.FriendlyName(),
                    Type = "array",
                    Items = new Items
                    {
                        Ref = (isComplex)? colMapping.Type : "",
                        Type = (!isComplex)? colMapping.Type : ""
                    }
                };
            }

            if (returnType.IsPrimitive)
            {
                return new PropertyType
                {
                    Description = returnType.FriendlyName(),
                    Type = returnType.Name
                };
            }

            var modelSpec = new ModelSpec { id = ModelIdFromType(returnType) };
            _models.Add(returnType, modelSpec);
            
            foreach (var prop in returnType.GetProperties())
            {
                var mapping = Register(prop.PropertyType, depth++);
                modelSpec.properties.Add(prop.Name, mapping);
            }

            return new PropertyType
            {
                Description = returnType.FriendlyName(),
                Type = modelSpec.id,
            };
        }

        private static string ModelIdFromType(Type type)
        {
            return type.FullName;
        }

        public static bool IsTypeSwaggerPrimitive(Type type)
        {
            type = Nullable.GetUnderlyingType(type) ?? type;
            return type.IsPrimitive || PrimitiveMappings.ContainsKey(type);
        }

        private static readonly Dictionary<Type, PropertyTypeMapping> PrimitiveMappings = new Dictionary<Type, PropertyTypeMapping>
        {
            {typeof (int), new PropertyTypeMapping("integer", "int32")},
            {typeof (long), new PropertyTypeMapping("integer", "int64")},
            {typeof (float), new PropertyTypeMapping("number", "float")},
            {typeof (double), new PropertyTypeMapping("number", "double")},
            {typeof (string), new PropertyTypeMapping("string")},
            {typeof (byte), new PropertyTypeMapping("string", "byte")},
            {typeof (bool), new PropertyTypeMapping("boolean")},
            {typeof (DateTime), new PropertyTypeMapping("string", "date-time")},
            {typeof (TimeSpan), new PropertyTypeMapping("string", "time-span")},
            {typeof (DateTimeOffset), new PropertyTypeMapping("string", "time-span")},
            {typeof (OperationResult), new PropertyTypeMapping("unknown")},
        };
    }
}