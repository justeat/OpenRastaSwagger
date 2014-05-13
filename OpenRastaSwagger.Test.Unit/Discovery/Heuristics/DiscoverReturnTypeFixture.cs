using NUnit.Framework;
using OpenRasta.Configuration.MetaModel;
using OpenRastaSwagger.Discovery;
using OpenRastaSwagger.Discovery.Heuristics;
using OpenRastaSwagger.DocumentationSupport;

namespace OpenRastaSwagger.Test.Unit.Discovery.Heuristics
{
    [TestFixture]
    class DiscoverReturnTypeFixture
    {
        private DiscoverReturnType _sut;
        private OperationMetadata _metadata;

        [SetUp]
        public void SetUp()
        {
            _sut = new DiscoverReturnType();

            _metadata = new OperationMetadata(new UriModel { Uri = "/some/uri" });
        }

        [Test]
        public void CanReflectReturnType()
        {
            var methodToDetect = typeof(TestHandler).GetMethod("Get");

            _sut.Discover(methodToDetect, _metadata);

            Assert.That(_metadata.ReturnType, Is.EqualTo(typeof(GetResponse)));
        }

        [Test]
        public void CanGetReturnTypeWhenOverloadedByAttribute()
        {
            var methodToDetect = typeof(TestHandler).GetMethod("Post");

            _sut.Discover(methodToDetect, _metadata);

            Assert.That(_metadata.ReturnType, Is.EqualTo(typeof(PostResponse)));
        }

        public class TestHandler
        {
            public GetResponse Get() { return null; }

            [ResponseTypeIs(typeof(PostResponse))]
            public void Post() { }
        }

        public class GetResponse {}
        public class PostResponse { }
    }
}