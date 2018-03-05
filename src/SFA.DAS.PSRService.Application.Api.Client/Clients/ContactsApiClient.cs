using System.Net.Http;
using System.Threading.Tasks;
using SFA.DAS.PSRService.Api.Types.Models;

namespace SFA.DAS.PSRService.Application.Api.Client.Clients
{
    public class ContactsApiClient : ApiClientBase, IContactsApiClient
    {
        public ContactsApiClient(string baseUri, ITokenService tokenService) : base(baseUri, tokenService)
        {
        }

        public async Task<Report> GetByUsername(string username)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/contacts/user/{username}"))
            {
                return await RequestAndDeserialiseAsync<Report>(request, $"Could not find the report");
            }
        }

        public async Task<Report> Create(CreateReportRequest report)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Post, $"/api/v1/contacts/"))
            {
                return await PostPutRequestWithResponse<CreateReportRequest, Report>(request, report);
            }
        }

        public async Task<Report> Update(UpdateReportRequest updateReportRequest)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Put, $"/api/v1/contacts/"))
            {
                return await PostPutRequestWithResponse<UpdateReportRequest, Report>(request, updateReportRequest);
            }
        }
    }

    public interface IContactsApiClient
    {
        Task<Report> GetByUsername(string username);

        Task<Report> Create(CreateReportRequest report);

        Task<Report> Update(UpdateReportRequest updateReportRequest);
    }
}