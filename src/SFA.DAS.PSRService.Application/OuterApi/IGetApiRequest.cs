using Newtonsoft.Json;

namespace SFA.DAS.PSRService.Application.OuterApi;

public interface IGetApiRequest 
{
    [JsonIgnore]
    string GetUrl { get; }
}