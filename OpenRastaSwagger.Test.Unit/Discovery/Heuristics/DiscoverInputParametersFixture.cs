using NUnit.Framework;
using OpenRasta.Configuration.MetaModel;
using OpenRastaSwagger.Discovery;
using OpenRastaSwagger.Discovery.Heuristics;
using OpenRastaSwagger.DocumentationSupport;

namespace OpenRastaSwagger.Test.Unit.Discovery.Heuristics
{
    [TestFixture]
    class DiscoverInputParametersFixture
    {
        private DiscoverInputParameters _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new DiscoverInputParameters();
        }

        [Test]
        public void CanFindPathParam()
        {
            var methodToDetect = typeof(TestHandler).GetMethod("GetString");

            var metadata = new OperationMetadata(new UriModel { Uri = "/some/uri/{s}" });

            _sut.Discover(methodToDetect, metadata);

            Assert.That(metadata.InputParameters[0].LocationType, Is.EqualTo(InputParameter.LocationTypes.Path));
        }

        [Test]
        public void CanFindQueryParam()
        {
            var methodToDetect = typeof(TestHandler).GetMethod("GetString");

            var metadata = new OperationMetadata(new UriModel { Uri = "/some/uri?s={s}" });

            _sut.Discover(methodToDetect, metadata);

            Assert.That(metadata.InputParameters[0].LocationType, Is.EqualTo(InputParameter.LocationTypes.Query));
        }

        [Test]
        public void CanFindPathParamForPrimativeType()
        {
            var methodToDetect = typeof(TestHandler).GetMethod("GetInt");

            var metadata = new OperationMetadata(new UriModel { Uri = "/some/uri/{i}" });

            _sut.Discover(methodToDetect, metadata);

            Assert.That(metadata.InputParameters[0].LocationType, Is.EqualTo(InputParameter.LocationTypes.Path));
        }

        [Test]
        public void CanFindQueryParamForPrimativeType()
        {
            var methodToDetect = typeof(TestHandler).GetMethod("GetInt");

            var metadata = new OperationMetadata(new UriModel { Uri = "/some/uri?s={i}" });

            _sut.Discover(methodToDetect, metadata);

            Assert.That(metadata.InputParameters[0].LocationType, Is.EqualTo(InputParameter.LocationTypes.Query));
        }

        [Test]
        public void CanFindHeaderParams()
        {
            var methodToDetect = typeof(TestHandler).GetMethod("GetString");

            var metadata = new OperationMetadata(new UriModel { Uri = "/some" });

            _sut.Discover(methodToDetect, metadata);

            Assert.That(metadata.InputParameters[1].LocationType, Is.EqualTo(InputParameter.LocationTypes.Header));
            Assert.That(metadata.InputParameters[1].Name, Is.EqualTo("A required header name"));
            Assert.That(metadata.InputParameters[1].Type, Is.EqualTo(typeof(int)));
            Assert.That(metadata.InputParameters[1].IsRequired, Is.EqualTo(true));

            Assert.That(metadata.InputParameters[2].LocationType, Is.EqualTo(InputParameter.LocationTypes.Header));
            Assert.That(metadata.InputParameters[2].Name, Is.EqualTo("A header name"));
            Assert.That(metadata.InputParameters[2].Type, Is.EqualTo(typeof(string)));
            Assert.That(metadata.InputParameters[2].IsRequired, Is.EqualTo(false));
        }

        [Test]
        public void IfComplexTypeAndSpecifiedInPath()
        {
            var methodToDetect = typeof(TestHandler).GetMethod("GetRequest");

            var metadata = new OperationMetadata(new UriModel { Uri = "/some/uri/{req}" });

            _sut.Discover(methodToDetect, metadata);

            Assert.That(metadata.InputParameters[0].LocationType, Is.EqualTo(InputParameter.LocationTypes.Path));
            Assert.That(metadata.InputParameters[0].Type, Is.EqualTo(typeof(string)));
        }

        [Test]
        public void IfComplexTypeAndSpecifiedInQuery()
        {
            var methodToDetect = typeof(TestHandler).GetMethod("GetRequest");

            var metadata = new OperationMetadata(new UriModel { Uri = "/some/uri?req={req}" });

            _sut.Discover(methodToDetect, metadata);

            Assert.That(metadata.InputParameters[0].LocationType, Is.EqualTo(InputParameter.LocationTypes.Query));
            Assert.That(metadata.InputParameters[0].Type, Is.EqualTo(typeof(string)));
        }

        [Test]
        public void InfersSimpleParamToBeQueryStringWhenNotSpecified()
        {
            var methodToDetect = typeof(TestHandler).GetMethod("GetString");

            var metadata = new OperationMetadata(new UriModel { Uri = "/some/uri" });

            _sut.Discover(methodToDetect, metadata);

            Assert.That(metadata.InputParameters[0].LocationType, Is.EqualTo(InputParameter.LocationTypes.Query));
        }

        [Test]
        public void InfersComplexParamToBeBody()
        {
            var methodToDetect = typeof(TestHandler).GetMethod("GetRequest");

            var metadata = new OperationMetadata(new UriModel { Uri = "/some/uri" });

            _sut.Discover(methodToDetect, metadata);

            Assert.That(metadata.InputParameters[0].LocationType, Is.EqualTo(InputParameter.LocationTypes.Body));
        }

        public class TestHandler
        {
            public GetResponse GetInt(int i) { return null; }

            [InputHeader("A header name", typeof(string))]
            [InputHeader("A required header name", typeof(int), true)]
            public GetResponse GetString(string s) { return null; }

            public GetResponse GetRequest(ComplexRequest req) { return null; }
        }

        public class GetResponse {}
        public class PostResponse { }

        public class ComplexRequest
        {
            public string S { get; set; }
        }
    }
}