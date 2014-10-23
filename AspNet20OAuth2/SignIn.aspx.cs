using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Policy;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AspNet20OAuth2
{
    public partial class SignIn : System.Web.UI.Page
    {
        private Uri GetCallbackUrl()
        {
            var requestUrl = Request.Url.AbsoluteUri;
            var pathAndQuery = Request.Url.PathAndQuery;
            var callbackPage = "/SignInCallback.aspx";

            var callbackUrl = requestUrl.Replace(pathAndQuery, callbackPage);
            
            return new Uri(callbackUrl);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            using (var client = new WebClient())
            {
                client.Headers["Accept"] = "application/json";
                string result = client.DownloadString("http://localhost:58325/api/values");
                // now use a JSON parser to parse the resulting string back to some CLR object
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            var authorizeEndpoint = new Uri("http://localhost:3333/core/connect/authorize"); //the login page of the IdSvr
            var clientId = "net2client"; //the id of the client i.e. MB website
            //openid scope means 'please authenticate'
            //profile means 'give me back the user's profile info'
            //read is defined by the client to request read access
            //clients can define their own scopes
            var scopes = "openid profile read idmgr"; 
            var state = Guid.NewGuid().ToString("N");
            var nonce = Guid.NewGuid().ToString("N");
            var redirectUri = GetCallbackUrl();
            var responseType = "id_token token";
            var authorizationUrl = GetAuthorizeUrl(authorizeEndpoint, redirectUri, clientId,
                scopes, state, responseType, nonce);

            //state is generated for each request.  We check that the response contains the same value for state.
            //if it doesn't we shouldn't continue as the response is not linked with our request and it should not be trusted.
            Session[Constants.OAuth2State] = state;

            Response.Redirect(authorizationUrl.AbsoluteUri);
        }

        public static Uri GetAuthorizeUrl(Uri authorizeEndpoint, Uri redirectUri, string clientId,
            string scopes, string state, string responseType, string nonce)
        {
            var queryString = string.Format("?client_id={0}&scope={1}&redirect_uri={2}&state={3}&response_type={4}&nonce={5}&response_mode=form_post",
                clientId,
                scopes,
                redirectUri,
                state,
                responseType,
                nonce);
            
            return new Uri(authorizeEndpoint.AbsoluteUri + queryString);
        }

        protected void Button3_Click(object sender, EventArgs e)
        {

        }
    }
}