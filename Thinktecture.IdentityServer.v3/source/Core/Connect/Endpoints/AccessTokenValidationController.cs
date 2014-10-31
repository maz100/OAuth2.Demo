﻿/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license
 */

using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Thinktecture.IdentityServer.Core.Configuration;
using Thinktecture.IdentityServer.Core.Extensions;
using Thinktecture.IdentityServer.Core.Hosting;
using Thinktecture.IdentityServer.Core.Logging;

namespace Thinktecture.IdentityServer.Core.Connect
{
    [RoutePrefix(Constants.RoutePaths.Oidc.AccessTokenValidation)]
    [NoCache]
    public class AccessTokenValidationController : ApiController
    {
        private readonly static ILog Logger = LogProvider.GetCurrentClassLogger();
        private readonly TokenValidator _validator;
        private readonly IdentityServerOptions _options;

        public AccessTokenValidationController(TokenValidator validator, IdentityServerOptions options)
        {
            _validator = validator;
            _options = options;
        }

        [Route]
        public async Task<IHttpActionResult> Get()
        {
            Logger.Info("Start access token validation request");

            if (!_options.AccessTokenValidationEndpoint.IsEnabled)
            {
                Logger.Warn("Endpoint is disabled. Aborting");
                return NotFound();
            }

            var parameters = Request.RequestUri.ParseQueryString();

            var token = parameters.Get("token");
            if (token.IsMissing())
            {
                Logger.Error("token is missing.");
                return BadRequest("token is missing.");
            }

            var result = await _validator.ValidateAccessTokenAsync(token, parameters.Get("expectedScope"));
            
            if (result.IsError)
            {
                Logger.Info("Returning error: " + result.Error);
                return BadRequest(result.Error);
            }

            var response = result.Claims.ToClaimsDictionary();
            Logger.Debug(JsonConvert.SerializeObject(response, Formatting.Indented));

            Logger.Info("Returning access token claims");
            return Json(response);
        }
    }
}