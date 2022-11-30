using Newtonsoft.Json;

namespace SFA.DAS.PSRService.Application.OuterApi.Requests
{
    public interface IGetApiRequest 
    {
        [JsonIgnore]
        string GetUrl { get; }
    }
}