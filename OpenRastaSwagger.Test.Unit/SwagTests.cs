using System;
using NUnit.Framework;
using OpenRasta.Configuration;
using OpenRasta.Configuration.MetaModel;
using OpenRasta.DI;
using OpenRasta.DI.Internal;
using OpenRasta.Pipeline;
using OpenRastaSwagger.SampleApi.Handlers;
using OpenRastaSwagger.SampleApi.Resources;

namespace OpenRastaSwagger.Test.Unit
{
    [TestFixture]
    public class SwagTests
    {
        private Swag _swag;

        [SetUp]
        public void SetUp()
        {
            var resolver = new InternalDependencyResolver();
            resolver.Registrations.Add(new DependencyRegistration(typeof(IMetaModelRepository), typeof(MetaModelRepository), new SingletonLifetimeManager(resolver)));
            resolver.Registrations.Add(new DependencyRegistration(typeof(IPipeline), typeof(PipelineRunner), new SingletonLifetimeManager(resolver)));
            resolver.Registrations.Add(new DependencyRegistration(typeof(IDependencyResolver), typeof(InternalDependencyResolver), new SingletonLifetimeManager(resolver)));
            var @defaults = new DefaultDependencyRegistrar();
            @defaults.Register(resolver);

            DependencyManager.UnsetResolver();
            DependencyManager.SetResolver(resolver);
            DependencyManager.GetService<IPipeline>();

            _swag = new Swag();
        }

        [Test]
        public void Discover_ReturnsSwaggerSpec()
        {
            var mmr = ConfigureOpenRasta(() =>
            {
                using (OpenRastaConfiguration.Manual)
                {
                    ResourceSpace.Has.ResourcesOfType<Home>().AtUri("/home")
                        .HandledBy<HomeHandler>().AsXmlSerializer();
                }
            });

            var spec = _swag.Discover(mmr);

            Assert.That(spec, Is.Not.Null);
        }

        public IMetaModelRepository ConfigureOpenRasta(Action config)
        {
            config();
            return DependencyManager.GetService<IMetaModelRepository>();
        }
    }
}
