using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NUnit.Framework;
using RegularApi.Controllers.Dashboard.Models;
using RegularApi.Controllers.Deployment.Views;

namespace RegularApi.Tests.Controllers.Deployment
{
    public class ApplicationControllerTest : BaseIT
    {
        private const string APPLICATION_URI = "http://192.168.99.1:5000/application";

        [SetUp]
        public void SetUp()
        {
            CreateTestServer();
        }

        [Test]
        public async Task TestValidationErrors()
        {
            var applicationRequest = new ApplicationRequest();

            var responseMessage = await PerformPostAsync(applicationRequest, APPLICATION_URI);

            Assert.AreEqual(HttpStatusCode.BadRequest, responseMessage.StatusCode);
        }

        [Test]
        public async Task TestNewApplicationSetupAsync_Created()
        {
            var applicationResource = new ApplicationResource()
            {
                Name = "test-app",
                DockerSetupResource = new DockerSetupResource()
                {
                    ImageName = "image-name",
                    RegistryUrl = "registry-url",
                    EnvironmentVariables = new[] { new KeyValuePair<object, object>("key", "value") },
                    Ports = new[] { new KeyValuePair<object, object>("8080", "80") }

                },
                HostResources = new HostResource[]
                {
                    new HostResource()
                    {
                        Ip = "192.168.99.1",
                        Username = "root",
                        Password = "r00t"
                    },
                }
            };

            var responseMessage = await PerformPostAsync(applicationResource, APPLICATION_URI);
            //var response = await GetResponse<IActionResult>(responseMessage);

            // await DeleteApplication(applicationR.Id);

            //Assert.AreEqual(HttpStatusCode.OK, responseMessage.StatusCode);

            // Assert.NotNull(response.DeploymentId);
            // Assert.NotNull(response.Received);
            // Assert.AreEqual(applicationRequest.Name, response.Name);
            // Assert.AreEqual(applicationRequest.Tag, response.Tag);
        }

        // ------------------------------------------------------------------------------------------------

        private async Task<HttpResponseMessage> PerformPostAsync<T>(T request, string uri)
        {
            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var responseMessage = await HttpClient.PostAsync(uri, content);

            return responseMessage;
        }

        private async Task<T> GetResponse<T>(HttpResponseMessage responseMessage)
        {
            var content = await responseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}