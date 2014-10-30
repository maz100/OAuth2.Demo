using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AspNet20OAuth2
{
    public partial class SignInCallback : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //check that the state querystring parameter received from the IdSvr
            //matches what we are expecting - we saved it in session
            ValidateState();

            RetrieveTokens();
        }

        private void ValidateState()
        {
            var msg = "invalid state";

            //we set the state session value when we redirected to the auth server
            //this is the callback from that request.  Now we must check the state matches
            //state checks are part of the OAuth2 spec
            //we should implement them whenever we call the OAuth server
            var responseState = Request.Form["state"];

            if (Session[Constants.OAuth2State] == null)
            {
                throw new Exception(msg);
            }

            var requestState = Session[Constants.OAuth2State].ToString();

            if (responseState != requestState)
            {
                throw new Exception(msg);
            }
        }

        private void RetrieveTokens()
        {
            var id_token = Request.Form["id_token"];
            var access_token = Request.Form["token"];

            //tokens are posted back here so we can get them from the form
            var sb = new StringBuilder();
            sb.AppendLine("Received identity data from OAuth2 service: ");
            sb.AppendLine("<br>");
            sb.AppendLine(string.Format("id_token: {0}", id_token));
            sb.AppendLine("<br>");
            sb.AppendLine(string.Format("signed access token: {0}", access_token));
            sb.AppendLine("<br>");
            sb.AppendLine(string.Format("access token: {0}", CheckToken(access_token)));

            var nonce = Session["nonce"].ToString();
            CheckIdToken(id_token, nonce);

            Literal1.Text = sb.ToString();
        }

        private string CheckToken(string token)
        {
            string authorizeUrl = string.Format("http://localhost:3333/core/connect/accesstokenvalidation?token={0}", token);

            WebClient client = new WebClient();
            
            Stream data = client.OpenRead(authorizeUrl);
            StreamReader reader = new StreamReader(data);
            string s = reader.ReadToEnd();
            Console.WriteLine(s);
            data.Close();
            reader.Close();

            return s;
        }

        private string CheckIdToken(string token, string nonce)
        {
            string authorizeUrl = string.Format("http://localhost:55461/validate?token={0}&nonce={1}", token, nonce);

            WebClient client = new WebClient();
            
            Stream data = client.OpenRead(authorizeUrl);
            StreamReader reader = new StreamReader(data);
            string s = reader.ReadToEnd();
            Console.WriteLine(s);
            data.Close();
            reader.Close();

            return s;
        }

    }
}