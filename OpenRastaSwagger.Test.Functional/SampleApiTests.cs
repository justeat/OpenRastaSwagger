using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenRasta.Hosting.InMemory;
using OpenRasta.Web;
using OpenRastaSwagger.SampleApi;
using OpenRastaSwagger.SampleApi.Resources;

namespace OpenRastaSwagger.Test.Functional
{

    [TestFixture]
    public class SampleApiTests
    {

        [Test]
        public void CanRetrieveHome()
        {
            using (var host = new InMemoryHost(new Configuration()))
            {
                var request = new InMemoryRequest
                {
                    Uri = new Uri("http://localhost/home"),
                    HttpMethod = "GET",
                    Entity = {ContentType = MediaType.Json}
                };

                request.Entity.Headers["Accept"] = "application/json";

                var response = host.ProcessRequest(request);
                var statusCode = response.StatusCode;
                Assert.AreEqual(200, statusCode);

                if (response.Entity.ContentLength > 0)
                {
                    response.Entity.Stream.Seek(0, SeekOrigin.Begin);

                    var serializer = new DataContractJsonSerializer(typeof(Home));
                    var home = (Home) serializer.ReadObject(response.Entity.Stream);

                    Assert.AreEqual("Welcome home.", home.Title);
                }
            }            

        }

    }
}
