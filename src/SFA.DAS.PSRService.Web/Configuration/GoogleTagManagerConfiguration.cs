using Newtonsoft.Json;

namespace SFA.DAS.PSRService.Web.Configuration
{
    public class GoogleTagManagerConfiguration
    {
        [JsonRequired]
        public string TrackingManagerCode { get; set; }
    }
}
