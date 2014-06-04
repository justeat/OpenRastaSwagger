using System;
using System.IO;
using System.Runtime.Serialization.Json;
using NUnit.Framework;
using OpenRasta.Hosting.InMemory;
using OpenRasta.Web;
using OpenRastaSwagger.Model.ResourceDetails;

namespace OpenRastaSwagger.Test.Functional
{
    [TestFixture]
    public class SampleApiTests
    {
        [Test]
        public void CanRetrieveSimpleResourceDetails()
        {
            using (var host = new InMemoryHost(new SampleApi.Configuration()))
            {
                var request = new InMemoryRequest
                {
                    Uri = new Uri("http://localhost/api-docs/simple"),
                    HttpMethod = "GET",
                    Entity = {ContentType = MediaType.Json}
                };

                request.Entity.Headers["Accept"] = "application/json";

                var response = host.ProcessRequest(request);
                var statusCode = response.StatusCode;
                Assert.AreEqual(200, statusCode);

                Assert.IsTrue(response.Entity.ContentLength>0);

                response.Entity.Stream.Seek(0, SeekOrigin.Begin);

                var serializer = new DataContractJsonSerializer(typeof(ResourceDetails));
                var resourceDetails = (ResourceDetails) serializer.ReadObject(response.Entity.Stream);

                Assert.IsNotNull(resourceDetails);
            }
        }
    }
}
