using System.IdentityModel.Tokens;
using Microsoft.Owin;
using OAuthHelperApi;
using Owin;
using Thinktecture.IdentityModel.Owin.ScopeValidation;
using Thinktecture.IdentityModel.Tokens;

[assembly: OwinStartup(typeof(Startup))]

namespace OAuthHelperApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseWebApi(WebApiConfig.Register());
        }
    }
}