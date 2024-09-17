using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SFA.DAS.PSRService.Application.OuterApi;

public class OuterApiClient : IOuterApiClient
{
    private readonly HttpClient _httpClient;
    private readonly OuterApiConfiguration _configuration;

    public OuterApiClient(HttpClient httpClient, OuterApiConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _httpClient.BaseAddress = new Uri(configuration.BaseUrl);
    }
    
    public async Task<ApiResponse<TResponse>> Get<TResponse>(IGetApiRequest request)
    {
        using var requestMessage = new HttpRequestMessage(HttpMethod.Get, request.GetUrl);
        AddAuthenticationHeader(requestMessage);
            
        var response = await _httpClient.SendAsync(requestMessage).ConfigureAwait(false);

        return await ProcessResponse<TResponse>(response);
    }
    private void AddAuthenticationHeader(HttpRequestMessage httpRequestMessage)
    {
        httpRequestMessage.Headers.Add("Ocp-Apim-Subscription-Key", _configuration.Key);
        httpRequestMessage.Headers.Add("X-Version", "1");
    }
    private static async Task<ApiResponse<TResponse>> ProcessResponse<TResponse>(HttpResponseMessage response)
    {
        var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            
        var errorContent = "";
        var responseBody = (TResponse)default;

        if (response.IsSuccessStatusCode)
        {
            responseBody = JsonConvert.DeserializeObject<TResponse>(json);
        }
        else
        {
            errorContent = json;
        }

        return new ApiResponse<TResponse>(responseBody, response.StatusCode, errorContent);
    }
}