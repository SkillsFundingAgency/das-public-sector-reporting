using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SFA.DAS.PSRService.Api.Types;

namespace SFA.DAS.PSRService.Api.Client
{
    public class PsrsApiClient : IPsrsApiClient
    {
        private readonly IPsrsApiConfiguration _configuration;
        private readonly SecureHttpClient _httpClient;


        public PsrsApiClient(IPsrsApiConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new SecureHttpClient(configuration);
        }

        internal PsrsApiClient(IPsrsApiConfiguration configuration, SecureHttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task<PsrsStatisticsUpdatedMessage> GetStatistics()
        {
            var baseUrl = GetBaseUrl();
            var url = $"{baseUrl}api/statistics";
            var json = await _httpClient.GetAsync(url);

            return JsonConvert.DeserializeObject<PsrsStatisticsUpdatedMessage>(json);
        }

        public async Task<ICollection<ReportSubmittedMessage>> GetSubmittedReports()
        {
            var baseUrl = GetBaseUrl();
            var url = $"{baseUrl}api/submittedreports";
            var json = await _httpClient.GetAsync(url);

            return JsonConvert.DeserializeObject<ICollection<ReportSubmittedMessage>>(json);
        }

        private string GetBaseUrl()
        {
            return _configuration.ApiBaseUrl.EndsWith("/")
                ? _configuration.ApiBaseUrl
                : _configuration.ApiBaseUrl + "/";
        }
    }
}
