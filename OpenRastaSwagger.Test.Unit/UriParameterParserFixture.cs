using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace OpenRastaSwagger.Test.Unit
{
    [TestFixture]
    public class UriParameterParserFixture
    {
        [Test]
        public void CanParseQueryParam()
        {
            var parser = new UriParameterParser("simple?name={Name}");
            Assert.IsTrue(parser.HasQueryParam("name"));
        }

        [Test]
        public void CanParsePathParam()
        {
            var parser = new UriParameterParser("simple/{Name}");
            Assert.IsTrue(parser.HasPathParam("name"));
        }

        [Test]
        public void CanParsePathAndQueryParam()
        {
            var parser = new UriParameterParser("simple/{Name}?id={ID}");
            Assert.IsTrue(parser.HasPathParam("name"));
            Assert.IsTrue(parser.HasQueryParam("id"));
        }

    }
}
