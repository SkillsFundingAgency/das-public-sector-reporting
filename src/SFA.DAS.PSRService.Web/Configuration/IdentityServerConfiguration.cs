﻿namespace SFA.DAS.PSRService.Web.Configuration
{
    public class IdentityServerConfiguration
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Authority { get; set; }
        public string Scopes { get; set; }
        public string MapUniqueJsonKey { get; set; }
        public string ChangeEmailLink { get; set; }
        public string ChangePasswordLink { get; set; }
        public ClaimIdentifierConfiguration ClaimIdentifierConfiguration { get; set; }
    }
}
