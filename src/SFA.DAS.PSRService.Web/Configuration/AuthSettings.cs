using Newtonsoft.Json;

namespace SFA.DAS.PSRService.Web.Configuration;

public class AuthSettings
{
    [JsonRequired]
    public string WtRealm { get; set; }
    [JsonRequired]
    public string MetadataAddress { get; set; }
    [JsonRequired]
    public string Role { get; set; }
}