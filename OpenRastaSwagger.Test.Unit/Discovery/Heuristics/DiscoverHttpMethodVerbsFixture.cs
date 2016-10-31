using NUnit.Framework;
using NUnit.Framework.Constraints;
using OpenRasta.Configuration.MetaModel;
using OpenRasta.Web;
using OpenRastaSwagger.Discovery;
using OpenRastaSwagger.Discovery.Heuristics;

namespace OpenRastaSwagger.Test.Unit.Discovery.Heuristics
{
    [TestFixture]
    public class DiscoverHttpMethodVerbsFixture
    {
        private DiscoverHttpMethodVerbs _sut;
        private OperationMetadata _metadata;

        [SetUp]
        public void SetUp()
        {
            _sut = new DiscoverHttpMethodVerbs();
            _metadata = new OperationMetadata(new UriModel { Uri = "/some/uri" }, null);
        }
        
        [TestCase("Get", "GET")]
        [TestCase("Post", "POST")]
        [TestCase("Put", "PUT")]
        [TestCase("Delete", "DELETE")]
        [TestCase("Head", "HEAD")]
        [TestCase("Options", "OPTIONS")]
        public void CanDiscoverVerbsBasedOnSimpleMethodNames(string methodName, string expectedVerb)
        {
            var methodToDetect = typeof(SampleHandler).GetMethod(methodName);

            _sut.Discover(methodToDetect, _metadata);

            Assert.That(_metadata.HttpVerb, Is.EqualTo(expectedVerb));
        }

        [TestCase("GetMyThing", "GET")]
        [TestCase("PostMyThing", "POST")]
        [TestCase("PutMyThing", "PUT")]
        [TestCase("DeleteMyThing", "DELETE")]
        [TestCase("HeadMyThing", "HEAD")]
        [TestCase("OptionsMyThing", "OPTIONS")]
        public void CanDiscoverVerbsBasedOnPrefixedMethodNames(string methodName, string expectedVerb)
        {
            var methodToDetect = typeof(SampleHandler).GetMethod(methodName);

            _sut.Discover(methodToDetect, _metadata);

            Assert.That(_metadata.HttpVerb, Is.EqualTo(expectedVerb));
        } 
        
        [Test]
        public void CanDiscoverVerbFromAttribute()
        {
            var methodToDetect = typeof(SampleHandler).GetMethod("SomeWeirdName");

            _sut.Discover(methodToDetect, _metadata);

            Assert.That(_metadata.HttpVerb, Is.EqualTo("GET"));
        }

        [Test]
        public void CanNotDiscoverNotStandardMethods()
        {
            var methodToDetect = typeof(SampleHandler).GetMethod("SomeOtherWeirdName");

            Assert.IsFalse(_sut.Discover(methodToDetect, _metadata));
        }

        [Test]
        public void NamesMatchCorrectly()
        {
            var method1 = typeof(SampleHandler).GetMethod("NameTest1");
            var method2 = typeof(SampleHandler).GetMethod("NameTest2");
            var metadata = new OperationMetadata(new UriModel { Uri = "/some/uri", Name = "NameTest1" }, null);

            Assert.IsTrue(_sut.Discover(method1, metadata));
            Assert.IsFalse(_sut.Discover(method2, metadata));
        }

        
        public class SampleHandler
        {
            public void Get() {}
            public void Post() {}
            public void Put() { }
            public void Delete() { }
            public void Head() { }
            public void Options() { }

            public void GetMyThing() {}
            public void PostMyThing() { }
            public void PutMyThing() { }
            public void DeleteMyThing() { }
            public void HeadMyThing() { }
            public void OptionsMyThing() { }

            [HttpOperation(HttpMethod.GET)]
            public void SomeWeirdName() { }

            public void SomeOtherWeirdName() { }

            [HttpOperation(HttpMethod.GET, ForUriName = "NameTest1")]
            public void NameTest1()
            {
            }

            [HttpOperation(HttpMethod.GET, ForUriName = "NameTest2")]
            public void NameTest2()
            {
            }
        }
    }
}
