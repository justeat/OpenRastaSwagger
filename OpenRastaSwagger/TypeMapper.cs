using System;
using System.Collections;
using System.Collections.Generic;
using OpenRasta.TypeSystem.ReflectionBased;
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
                type = mapping.type,
                format = mapping.format,
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
                    type = _models[returnType].id,
                    items = new Items()
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

                    type = mapping.Type,
                    format = mapping.Format
                };
            }

            if (returnType.Implements<IEnumerable>())
            {
                Type collectionType = returnType.GetElementType();

                if (returnType.IsGenericType)
                {
                    collectionType = returnType.GetGenericArguments()[0];
                }

                var colMapping = Register(collectionType, depth++);

                bool isComplex = _models.ContainsKey(collectionType);

                return new PropertyType()
                {
                    type = "array",
                    items = new Items()
                    {
                        Ref = (isComplex)? colMapping.type : "",
                        Type = (!isComplex)? colMapping.type : ""
                    }
                };
            }

            if (returnType.IsPrimitive)
            {
                return new PropertyType()
                {
                    type = returnType.Name
                };
            }

            var modelSpec = new ModelSpec {id = returnType.Name};
            _models.Add(returnType, modelSpec);


            foreach (var prop in returnType.GetProperties())
            {
                var mapping = Register(prop.PropertyType, depth++);
                modelSpec.properties.Add(prop.Name, mapping);
            }

            return new PropertyType()
            {
                type = modelSpec.id,
            };
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
        };
    }

    public class PropertyTypeMapping
    {
        public string Type { get; set; }
        public string Format { get; set; }

        public PropertyTypeMapping(string type, string format="")
        {
            Type = type;
            Format = format;
        }

    }


}