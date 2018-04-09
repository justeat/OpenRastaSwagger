using System;
using System.Reflection;
using NUnit.Framework;
using OpenRasta.Configuration.MetaModel;
using OpenRasta.TypeSystem.ReflectionBased;
using OpenRasta.Web;
using OpenRastaSwagger.Discovery;
using OpenRastaSwagger.DocumentationSupport;
using OpenRastaSwagger.Grouping;
using OpenRastaSwagger.SampleApi.Handlers;
using OpenRastaSwagger.SampleApi.Resources;

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
            _model.Uris.Add(new UriModel { Name = "Test", Uri = "/test-with-attributes" });
            _model.Handlers.Add(new HandlerModel(new ReflectionBasedType(new ReflectionBasedTypeSystem(), typeof(TestHandler))));
            AddHandlerResourceType(typeof (OperationResult));
        }

        [Test]
        public void TestHandlerProvided_DiscoveryRulesExecutedOnType()
        {
            var heuristic = new FakeDiscoveryHeuristic();
            _discoverer.DiscoveryRules.Clear();
            _discoverer.DiscoveryRules.Add(heuristic);

            _discoverer.Discover(_model);

            Assert.That(heuristic.Called, Is.True);
            Assert.That(heuristic.MethodInfo, Is.EqualTo(typeof(TestHandler).GetMethod("GetInt2")));
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
        public void HandlerHasMoreResourcesThanRegistered_OnlyReturnsRegisteredResources()
        {
            AddHandlerResourceType(typeof(int));
            var metadata = _discoverer.Discover(_model);

            Assert.AreEqual(1, metadata.Count);
            Assert.That(typeof(int) == metadata[0].ReturnType);
        }

        [Test]
        public void HandlerHasMoreResourcesThanRegisteredUsingAttribute_OnlyReturnsRegisteredResources()
        {
            AddHandlerResourceType(typeof(int));
            _model.Handlers.Clear();
            _model.Handlers.Add(new HandlerModel(new ReflectionBasedType(new ReflectionBasedTypeSystem(), typeof(TestHandlerWithAttribute))));

            var metadata = _discoverer.Discover(_model);

            Assert.AreEqual(1, metadata.Count);
            Assert.That(typeof(int) == metadata[0].ReturnType);
        }

        [Test]
        public void HandlerWithoutDescriptionAttribute_SetsTheSummaryToBlankString()
        {
            _model.Handlers.Clear();
            _model.Handlers.Add(new HandlerModel(new ReflectionBasedType(new ReflectionBasedTypeSystem(), typeof(SimpleHandler))));
            AddHandlerResourceType(typeof(SimpleResource));

            var metadata = _discoverer.Discover(_model);

            Assert.That(metadata[0].Summary, Is.EqualTo("SimpleHandler.Get"));
        }

        [Test]
        public void HandlerWithDescriptionAttribute_SetsTheSummaryToTheValueProvidedInTheDescription()
        {
            _model.Handlers.Clear();
            AddHandlerResourceType(typeof(string));
            _model.Handlers.Add(new HandlerModel(new ReflectionBasedType(new ReflectionBasedTypeSystem(), typeof(HandlerWithAttributes))));

            var metadata = _discoverer.Discover(_model);

            Assert.That(metadata[0].Summary, Is.EqualTo("The description for attribute handler"));
        }

        [Test]
        public void HandlerWithNoNotesAttribute_SetsTheNotesToTheUriTemplate()
        {
            _model.Handlers.Clear();
            AddHandlerResourceType(typeof(int));
            _model.Handlers.Add(new HandlerModel(new ReflectionBasedType(new ReflectionBasedTypeSystem(), typeof(TestHandlerWithAttribute))));

            var metadata = _discoverer.Discover(_model);

            Assert.That(metadata[0].Notes, Is.EqualTo("Uri template /test-with-attributes"));
        }

        [Test]
        public void HandlerWithNotesAttribute_SetsTheNotesToTheValueProvidedInTheNote()
        {
            _model.Handlers.Clear();
            AddHandlerResourceType(typeof(string));
            _model.Handlers.Add(new HandlerModel(new ReflectionBasedType(new ReflectionBasedTypeSystem(), typeof(HandlerWithAttributes))));

            var metadata = _discoverer.Discover(_model);

            Assert.That(metadata[0].Notes, Is.EqualTo("The notes for attribute handler"));
        }

        [Test]
        public void HandlerWithHyphenatedResource_SetsCorrectGroupPath()
        {
            _model.Handlers.Clear();
            AddHandlerResourceType(typeof(string));
            _model.Handlers.Add(new HandlerModel(new ReflectionBasedType(new ReflectionBasedTypeSystem(), typeof(HandlerWithAttributes))));

            var metadata = _discoverer.Discover(_model);

            Assert.That(metadata[0].Group.Path, Is.EqualTo("test-with-attributes"));
        }

        [Test]
        public void HandlerWithObsoleteMethod_DoesNotRecogniseAsAHandler()
        {
            _model.Handlers.Clear();
            _model.Handlers.Add(new HandlerModel(new ReflectionBasedType(new ReflectionBasedTypeSystem(), typeof(TestHandlerWithObsoleteAttribute))));

            var metadata = _discoverer.Discover(_model);

            Assert.That(metadata, Is.Empty);
        }

        public class TestHandler { public OperationResult GetInt(int i) { return null; } public int GetInt2(int i) { return 0; } }
        public class TestHandlerWithAttribute { public OperationResult GetInt(int i) { return null; } [ResponseTypeIs(typeof(int))] public OperationResult GetInt2(int i) { return new OperationResult.OK(0); } }
        public abstract class TestHandlerWithProperyThatShouldNotBeDiscovered { public string Something { get; set; } }
        public class TestHandlerDerivedFromAbstract : TestHandlerWithProperyThatShouldNotBeDiscovered { }

        public class TestHandlerWithObsoleteAttribute
        {
            [Obsolete]
            public OperationResult GetInt(int i)
            {
                return null;
            }
        }

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

        private void AddHandlerResourceType(Type type)
        {
            _model.ResourceKey = new ReflectionBasedType(new ReflectionBasedTypeSystem(null, null), type);
        }
    }
}
