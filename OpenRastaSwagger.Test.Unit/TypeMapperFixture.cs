using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        [TestCase(typeof(Int32), "integer", "int32")]
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

        [TestCase(typeof(IEnumerable<string>))]
        [TestCase(typeof(ICollection<string>))]
        [TestCase(typeof(Collection<string>))]
        [TestCase(typeof(List<string>))]
        [TestCase(typeof(IList<string>))]
        [TestCase(typeof(string[]))]
        public void CanMapEnumerableStringTypes(Type collectionType)
        {
            var mapper = new TypeMapper();

            var param = mapper.Register(collectionType);

            Assert.AreEqual("array", param.type);
            Assert.AreEqual("string", param.items.Type);

            Assert.IsEmpty(mapper.Models);
        }

        [TestCase(typeof(IEnumerable<int>))]
        [TestCase(typeof(ICollection<int>))]
        [TestCase(typeof(Collection<int>))]
        [TestCase(typeof(List<int>))]
        [TestCase(typeof(IList<int>))]
        [TestCase(typeof(int[]))]
        public void CanMapEnumerableIntegerTypes(Type collectionType)
        {
            var mapper = new TypeMapper();

            var param = mapper.Register(collectionType);

            Assert.AreEqual("array", param.type);
            Assert.AreEqual("integer", param.items.Type);

            Assert.IsEmpty(mapper.Models);
        }


        [TestCase(typeof(IEnumerable<ComplexObject>))]
        [TestCase(typeof(ICollection<ComplexObject>))]
        [TestCase(typeof(Collection<ComplexObject>))]
        [TestCase(typeof(List<ComplexObject>))]
        [TestCase(typeof(IList<ComplexObject>))]
        [TestCase(typeof(ComplexObject[]))]
        public void CanMapEnumerableComplexObjectTypes(Type collectionOfComplexType)
        {
            var mapper = new TypeMapper();

            var param = mapper.Register(collectionOfComplexType);

            Assert.AreEqual("array", param.type);
            Assert.AreEqual("ComplexObject", param.items.Ref);

            Assert.AreEqual(1, mapper.Models.Count());
            var model = mapper.Models.First();

            

        }

        public class ComplexObject
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public ComplexObject Parent { get; set; }
        }

    }
}
