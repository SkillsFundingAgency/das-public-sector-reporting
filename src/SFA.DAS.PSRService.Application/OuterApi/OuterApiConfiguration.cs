using Newtonsoft.Json;

namespace SFA.DAS.PSRService.Application.OuterApi
{
    public class OuterApiConfiguration
    {
        [JsonRequired]
        public string Key { get ; set ; }
        [JsonRequired]
        public string BaseUrl { get ; set ; }
    }
}