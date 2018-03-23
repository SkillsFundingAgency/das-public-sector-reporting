namespace SFA.DAS.PSRService.Web.Configuration
{
    public class IdentityServerConfiguration
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public int AuthenticationMethod { get; set; }
        public string Authority { get; set; }
        public string ResponseType { get; set; }
        public bool SaveTokens { get; set; }
        public string Scopes { get; set; }
        public string MapUniqueJsonKey { get; set; }
        public ClaimIdentifierConfiguration ClaimIdentifierConfiguration { get; set; }
    }
}
