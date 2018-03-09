using Newtonsoft.Json;

namespace SFA.DAS.PSRService.Web.Configuration
{
    public class WebConfiguration : IWebConfiguration
    {
        [JsonRequired]
        public AuthSettings Authentication { get; set; }
        [JsonRequired]
        public string SqlConnectionString { get; set; }
    }
}