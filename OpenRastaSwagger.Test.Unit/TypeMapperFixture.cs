using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenRastaSwagger.Discovery;

namespace OpenRastaSwagger.Test.Unit
{
    [TestFixture]
    public class TypeMapperFixture
    {
        [TestCase(typeof(string), "string")]
        [TestCase(typeof(String), "string")]
        [TestCase(typeof(int), "integer", "int32")]
        [TestCase(typeof(long), "integer", "int64")]
        [TestCase(typeof(float), "number", "float")]
        [TestCase(typeof(double), "number", "double")]
        [TestCase(typeof(bool), "boolean")]
        [TestCase(typeof(byte), "string", "byte")]
        [TestCase(typeof(DateTime), "string", "date-time")]
        public void CanMapSimpleTypes(Type type, string expectedParamType, string expectedFormat="")
        {
            var mapper=new TypeMapper();
            var inputParam = new InputParameter {Type = type};
            var param=mapper.Map(inputParam);

            Assert.AreEqual(expectedParamType, param.type);
            Assert.AreEqual(expectedFormat, param.format);

            Assert.IsEmpty(mapper.Models);

        }

        [TestCase(typeof(int?), "integer", "int32")]
        [TestCase(typeof(long?), "integer", "int64")]
        [TestCase(typeof(float?), "number", "float")]
        [TestCase(typeof(double?), "number", "double")]
        [TestCase(typeof(bool?), "boolean")]
        [TestCase(typeof(byte?), "string", "byte")]
        [TestCase(typeof(DateTime?), "string", "date-time")]
        public void CanMapSimpleNullableTypes(Type type, string expectedParamType, string expectedFormat = "")
        {
            var mapper = new TypeMapper();
            var inputParam = new InputParameter { Type = type };
            var param = mapper.Map(inputParam);

            Assert.AreEqual(expectedParamType, param.type);
            Assert.AreEqual(expectedFormat, param.format);

            Assert.IsEmpty(mapper.Models);

        }

    }
}
