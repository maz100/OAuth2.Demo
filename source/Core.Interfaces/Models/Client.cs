﻿using System;
using System.Collections.Generic;

namespace Thinktecture.IdentityServer.Core.Models
{
    public class Client
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string ClientName { get; set; }
        public string ClientUri { get; set; }
        public Uri LogoUri { get; set; }
        public ApplicationTypes ApplicationType { get; set; }
        public bool RequireConsent { get; set; }

        public Flows Flow { get; set; }
        public List<Uri> RedirectUris { get; set; }
        
        public int IdentityTokenLifetime { get; set; }
        public int AccessTokenLifetime { get; set; }

        public List<string> ScopeRestrictions { get; set; }

        public SubjectTypes SubjectType { get; set; }
        public Uri SectorIdentifierUri { get; set; }
        public SigningKeyTypes SigningKeyType { get; set; }
    }
}
