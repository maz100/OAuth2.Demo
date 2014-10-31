using System.Net;
using log4net;
using log4net.Config;
using NUnit.Framework;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Thinktecture.IdentityModel.Client;

namespace Test.SomeUsefulApi
{
    [TestFixture]
    public class UsefulControllerTests
    {
        private static readonly ILog Log = LogManager.GetLogger("UsefulControllerTests");
        [SetUp]
        public void SetUp()
        {
            XmlConfigurator.Configure();
        }

        [Test]
        public async void TestGet_valid_token_is_used_to_call_my_really_useful_api()
        {
            var accessToken = GetValidAccessToken();

            var response = await CallService(accessToken);

            Log.DebugFormat("Called service got result: {0}", response.Content.ReadAsStringAsync().Result);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async void TestGet_invalid_token_means_no_cigar()
        {
            var accessToken = GetInvalidAccessToken();

            var response = await CallService(accessToken);

            Log.DebugFormat("HttpStatusCode was: {0}", response.StatusCode);

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        public async Task<HttpResponseMessage> CallService(string accessToken)
        {
            var client = new HttpClient();

            client.SetBearerToken(accessToken);

            var api = "http://localhost:56679/Useful";

            var response = await client.GetAsync(api);

            return response;
        }

        private string GetValidAccessToken()
        {
            var authorizeEndpoint = new Uri("http://localhost:3333/core/connect/token");
            var client = new OAuth2Client(
                authorizeEndpoint,
                "roclient",
                "secret");

            var token = client.RequestResourceOwnerPasswordAsync("bob", "bob", "openid read write idmgr").Result;

            var accessToken = token.AccessToken;
          
            return accessToken;
        }

        private string GetInvalidAccessToken()
        {
            var accessToken = "some invalid token";

            return accessToken;
        }
    }
}
