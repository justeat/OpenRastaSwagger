using System.Reflection;
using NUnit.Framework;
using OpenRasta.Configuration.MetaModel;
using OpenRasta.TypeSystem.ReflectionBased;
using OpenRasta.Web;
using OpenRastaSwagger.Discovery;
using OpenRastaSwagger.Grouping;
using OpenRastaSwagger.SampleApi.Handlers;

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
        public void TestHandlerProvided_DiscoveryRulesExecutedOnType()
        {
            var heuristic = new FakeDiscoveryHeuristic();
            _discoverer.DiscoveryRules.Clear();
            _discoverer.DiscoveryRules.Add(heuristic);

            _discoverer.Discover(_model);

            Assert.That(heuristic.Called, Is.True);
            Assert.That(heuristic.MethodInfo, Is.EqualTo(typeof(TestHandler).GetMethod("GetInt")));
        }

        [Test]
        public void AbstractHandlerProvided_DoesNotRecogniseAsAHandler()
        {
            _model.Handlers.Clear();
            _model.Handlers.Add(new HandlerModel(new ReflectionBasedType(new ReflectionBasedTypeSystem(), typeof(TestHandlerWithProperyThatShouldNotBeDiscovered))));

            var metadata = _discoverer.Discover(_model);

            Assert.That(metadata, Is.Empty);
        }

        [Test]
        public void HandlerThatDerivesFromAbstractHandlerProvided_WithNoSuitableMethodPresent_DoesNotRecogniseAsAHandler()
        {
            _model.Handlers.Clear();
            _model.Handlers.Add(new HandlerModel(new ReflectionBasedType(new ReflectionBasedTypeSystem(), typeof(TestHandlerDerivedFromAbstract))));

            var metadata = _discoverer.Discover(_model);

            Assert.That(metadata, Is.Empty);
        }

        [Test]
        public void HandlerWithoutDescriptionAttribute_SetsTheSummaryToBlankString()
        {
            _model.Handlers.Clear();
            _model.Handlers.Add(new HandlerModel(new ReflectionBasedType(new ReflectionBasedTypeSystem(), typeof(SimpleHandler))));

            var metadata = _discoverer.Discover(_model);

            Assert.That(metadata[0].Summary, Is.EqualTo("SimpleHandler.Get"));
        }

        [Test]
        public void HandlerWithDescriptionAttribute_SetsTheSummaryToTheValueProvidedInTheDescription()
        {
            _model.Handlers.Clear();
            _model.Handlers.Add(new HandlerModel(new ReflectionBasedType(new ReflectionBasedTypeSystem(), typeof(HandlerWithAttributes))));

            var metadata = _discoverer.Discover(_model);

            Assert.That(metadata[0].Summary, Is.EqualTo("The description for attribute handler"));
        }

        public class TestHandler { public OperationResult GetInt(int i) { return null; } }
        public abstract class TestHandlerWithProperyThatShouldNotBeDiscovered { public string Something { get; set; } }
        public class TestHandlerDerivedFromAbstract : TestHandlerWithProperyThatShouldNotBeDiscovered { }

        public class FakeDiscoveryHeuristic : IDiscoveryHeuristic
        {
            public bool Called { get; set; }
            public MethodInfo MethodInfo { get; set; }

            public bool Discover(MethodInfo method, OperationMetadata methodMetdata)
            {
                Called = true;
                MethodInfo = method;
                return true;
            }
        }
    }
}
