using System.Linq;
using NUnit.Framework;
using OpenRasta.Configuration.MetaModel;
using OpenRasta.Web;
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

            var metadata = new OperationMetadata(new UriModel { Uri = "/some/uri/{s}" }, null);

            _sut.Discover(methodToDetect, metadata);

            Assert.That(metadata.InputParameters[0].LocationType, Is.EqualTo(InputParameter.LocationTypes.Path));
        }


        [Test]
        public void CanFindQueryParam()
        {
            var methodToDetect = typeof(TestHandler).GetMethod("GetString");

            var metadata = new OperationMetadata(new UriModel { Uri = "/some/uri?s={s}" }, null);

            _sut.Discover(methodToDetect, metadata);

            Assert.That(metadata.InputParameters[0].LocationType, Is.EqualTo(InputParameter.LocationTypes.Query));
        }

        [Test]
        public void CanFindPathParamForPrimativeType()
        {
            var methodToDetect = typeof(TestHandler).GetMethod("GetInt");

            var metadata = new OperationMetadata(new UriModel { Uri = "/some/uri/{i}" }, null);

            _sut.Discover(methodToDetect, metadata);

            Assert.That(metadata.InputParameters[0].LocationType, Is.EqualTo(InputParameter.LocationTypes.Path));
        }

        [Test]
        public void CanFindQueryParamForPrimativeType()
        {
            var methodToDetect = typeof(TestHandler).GetMethod("GetInt");

            var metadata = new OperationMetadata(new UriModel { Uri = "/some/uri?s={i}" }, null);

            _sut.Discover(methodToDetect, metadata);

            Assert.That(metadata.InputParameters[0].LocationType, Is.EqualTo(InputParameter.LocationTypes.Query));
        }

        [Test]
        public void CanFindHeaderParams()
        {
            var methodToDetect = typeof(TestHandler).GetMethod("GetString");

            var metadata = new OperationMetadata(new UriModel { Uri = "/some" }, null);

            _sut.Discover(methodToDetect, metadata);

            var param1 = metadata.InputParameters.First(x => x.Name == "A header name");
            var param2 = metadata.InputParameters.First(x => x.Name == "A required header name");

            Assert.That(param1.LocationType, Is.EqualTo(InputParameter.LocationTypes.Header));
            Assert.That(param1.Name, Is.EqualTo("A header name"));
            Assert.That(param1.Type, Is.EqualTo(typeof(string)));
            Assert.That(param1.IsRequired, Is.EqualTo(false));

            Assert.That(param2.LocationType, Is.EqualTo(InputParameter.LocationTypes.Header));
            Assert.That(param2.Name, Is.EqualTo("A required header name"));
            Assert.That(param2.Type, Is.EqualTo(typeof(int)));
            Assert.That(param2.IsRequired, Is.EqualTo(true));

        }

        [Test]
        public void IfComplexTypeAndSpecifiedInPath()
        {
            var methodToDetect = typeof(TestHandler).GetMethod("GetRequest");

            var metadata = new OperationMetadata(new UriModel { Uri = "/some/uri/{req}" }, null);

            _sut.Discover(methodToDetect, metadata);

            Assert.That(metadata.InputParameters[0].LocationType, Is.EqualTo(InputParameter.LocationTypes.Path));
            Assert.That(metadata.InputParameters[0].Type, Is.EqualTo(typeof(string)));
        }

        [Test]
        public void IfComplexTypeAndSpecifiedInQuery()
        {
            var methodToDetect = typeof(TestHandler).GetMethod("GetRequest");

            var metadata = new OperationMetadata(new UriModel { Uri = "/some/uri?req={req}" }, null);

            _sut.Discover(methodToDetect, metadata);

            Assert.That(metadata.InputParameters[0].LocationType, Is.EqualTo(InputParameter.LocationTypes.Query));
            Assert.That(metadata.InputParameters[0].Type, Is.EqualTo(typeof(string)));
        }

        [Test]
        public void InfersSimpleParamToBeQueryStringWhenNotSpecified()
        {
            var methodToDetect = typeof(TestHandler).GetMethod("GetString");

            var metadata = new OperationMetadata(new UriModel { Uri = "/some/uri" }, null);

            _sut.Discover(methodToDetect, metadata);

            Assert.That(metadata.InputParameters[0].LocationType, Is.EqualTo(InputParameter.LocationTypes.Query));
        }

        [Test]
        public void InfersComplexParamToBeBody()
        {
            var methodToDetect = typeof(TestHandler).GetMethod("GetRequest");

            var metadata = new OperationMetadata(new UriModel { Uri = "/some/uri" }, null);

            _sut.Discover(methodToDetect, metadata);

            Assert.That(metadata.InputParameters[0].LocationType, Is.EqualTo(InputParameter.LocationTypes.Body));
        }

        [Test]
        public void ParamsMatch()
        {
            var method1 = typeof(TestHandler).GetMethod("ParamTest1");
            var method2 = typeof(TestHandler).GetMethod("ParamTest2");

            var metadata = new OperationMetadata(new UriModel { Uri = "/some/uri/{myString}" }, null);

            Assert.That(_sut.Discover(method1, metadata), Is.True);
            Assert.That(_sut.Discover(method2, metadata), Is.False);
        }

        public class TestHandler
        {
            public GetResponse GetInt(int i) { return null; }


            [RequestHeader("A header name", typeof(string))]
            [RequestHeader("A required header name", typeof(int), true)]
            public GetResponse GetString(string s) { return null; }

            public GetResponse GetRequest(ComplexRequest req) { return null; }
            [HttpOperation(HttpMethod.GET)]
            public GetResponse ParamTest1(string myString)
            {
                return null;
            }

            [HttpOperation(HttpMethod.GET)]
            public GetResponse ParamTest2(int myInt)
            {
                return null;
            }
        }

        public class GetResponse {}
        public class PostResponse { }

        public class ComplexRequest
        {
            public string S { get; set; }
        }
    }
}