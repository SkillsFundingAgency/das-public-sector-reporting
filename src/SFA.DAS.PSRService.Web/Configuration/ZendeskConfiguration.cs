using Newtonsoft.Json;

namespace SFA.DAS.PSRService.Web.Configuration;

public class ZenDeskConfiguration
{
    [JsonRequired]
    public string SnippetKey { get; set; }
    [JsonRequired]
    public string SectionId { get; set; }
    [JsonRequired]
    public string CobrowsingSnippetKey { get; set; }
}