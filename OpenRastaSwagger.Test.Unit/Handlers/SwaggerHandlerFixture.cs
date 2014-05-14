using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using OpenRastaSwagger.Handlers;
using OpenRastaSwagger.Model.ResourceDetails;
using OpenRastaSwagger.Model.ResourceListing;

namespace OpenRastaSwagger.Test.Unit.Handlers
{
    [TestFixture]
    public class SwaggerHandlerFixture
    {
        private SwaggerHandler _handler;
        private Mock<ISwaggerDiscoverer> _mockDiscoverer;

        [SetUp]
        public void SetUp()
        {
            _mockDiscoverer = new Mock<ISwaggerDiscoverer>();
            _mockDiscoverer.Setup(x => x.ExcludedHandlers).Returns(new List<Type>());
            _handler = new SwaggerHandler(_mockDiscoverer.Object, new List<Type>());
        }

        [Test]
        public void HandlerCalledWithNoParams_GetsResourceList()
        {
            var list = new ResourceList();
            _mockDiscoverer.Setup(x => x.GetResourceList()).Returns(list);

            var response = _handler.Get();

            Assert.That(response, Is.EqualTo(list));
        }

        [Test]
        public void HandlerCalledWithParams_GetsResourceDetails()
        {
            var details = new ResourceDetails();
            _mockDiscoverer.Setup(x => x.GetResouceDetails("path")).Returns(details);

            var response = _handler.Get("path");

            Assert.That(response, Is.EqualTo(details));
        }
    }
}
