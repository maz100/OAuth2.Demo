using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using Newtonsoft.Json;
using Thinktecture.IdentityModel;
using System;

namespace OAuthHelperApi.Controllers
{
    public class ValidateController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(string token)
        {
            var result = ValidateToken(token);

            string json = JsonConvert.SerializeObject(result, Formatting.Indented,
                new JsonSerializerSettings
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                });
            
            return json;
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }

        private List<Claim> ValidateToken(string token)
        {
            var parameters = new TokenValidationParameters
            {
                ValidAudience = "net2client",
                ValidIssuer = "https://idsrv3.com",
                IssuerSigningToken = new X509SecurityToken(X509.LocalMachine.TrustedPeople.SubjectDistinguishedName.Find("CN=idsrv3test", false).First())
            };

            SecurityToken jwt;
            var id = new JwtSecurityTokenHandler().ValidateToken(token, parameters, out jwt);
            
            if (id.FindFirst("nonce").Value !=
               id.FindFirst("nonce").Value)
            {
                throw new InvalidOperationException("Invalid nonce");
            }

            return id.Claims.ToList();
        }
    }
}