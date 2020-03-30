using Newtonsoft.Json;

namespace SFA.DAS.PSRService.Web.Configuration
{
    public class ZendeskConfiguration
    {
        [JsonRequired]
        public string SnippetKey { get; set; }
        [JsonRequired]
        public string SectionId { get; set; }
    }
}
