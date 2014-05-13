using NUnit.Framework;
using OpenRasta.Configuration.MetaModel;
using OpenRasta.TypeSystem.ReflectionBased;
using OpenRasta.Web;
using OpenRastaSwagger.Discovery;
using OpenRastaSwagger.Grouping;

namespace OpenRastaSwagger.Test.Unit.Discovery
{
    [TestFixture]
    public class ResourceMetadataDiscovererFixture
    {
        private ResourceMetadataDiscoverer _discoverer;
        private ResourceModel _model;

        [SetUp]
        public void SetUp()
        {
            _discoverer = new ResourceMetadataDiscoverer(new OperationGrouperByUri());
            _model = new ResourceModel();
            _model.Uris.Add(new UriModel{Name = "Test", Uri = "/test"});
            _model.Handlers.Add(new HandlerModel(new ReflectionBasedType(new ReflectionBasedTypeSystem(), typeof(TestHandler))));
        }

        [Test]
        public void CanDiscoverMethodName()
        {
            var metadata = _discoverer.Discover(_model);

            Assert.That(metadata[0].Name, Is.EqualTo("GetInt"));
        }

        public class TestHandler
        {
            public OperationResult GetInt(int i) { return null; }
        }
    }
}
