using System.Net;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.PSRService.Application.EmployerUserAccounts;
using SFA.DAS.PSRService.Application.EmployerUserAccounts.Requests;
using SFA.DAS.PSRService.Application.EmployerUserAccounts.Responses;
using SFA.DAS.PSRService.Application.OuterApi;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.PSRService.Application.UnitTests.EmployerUserAccounts
{
    public class WhenGettingEmployerUserAccounts
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_And_Data_Returned(
            string email,
            string userId,
            GetUserAccountsResponse response,
            [Frozen] Mock<IOuterApiClient> apiClient,
            EmployerUserAccountsService service)
        {
            var expectedRequest = new GetUserAccountsRequest(userId, email);
            apiClient.Setup(x =>
                    x.Get<GetUserAccountsResponse>(
                        It.Is<GetUserAccountsRequest>(c => c.GetUrl.Equals(expectedRequest.GetUrl))))
                .ReturnsAsync(new ApiResponse<GetUserAccountsResponse>(response, HttpStatusCode.OK, ""));

            var actual = await service.GetEmployerUserAccounts(email, userId);

            actual.Should().BeEquivalentTo(response);
        }

        [Test, MoqAutoData]
        public async Task Then_If_Not_Found_Then_Empty_Returned(
            string email,
            string userId,
            GetUserAccountsResponse response,
            [Frozen] Mock<IOuterApiClient> apiClient,
            EmployerUserAccountsService service)
        {
            var expectedRequest = new GetUserAccountsRequest(userId, email);
            apiClient.Setup(x =>
                    x.Get<GetUserAccountsResponse>(
                        It.Is<GetUserAccountsRequest>(c => c.GetUrl.Equals(expectedRequest.GetUrl))))
                .ReturnsAsync(new ApiResponse<GetUserAccountsResponse>(new GetUserAccountsResponse(), HttpStatusCode.NotFound, "Not Found"));

            var actual = await service.GetEmployerUserAccounts(email, userId);

            actual.Should().BeEquivalentTo(new GetUserAccountsResponse());
        }
        
        [Test, MoqAutoData]
        public async Task Then_If_Not_Successful_Response_Null_Returned(
            string email,
            string userId,
            GetUserAccountsResponse response,
            [Frozen] Mock<IOuterApiClient> apiClient,
            EmployerUserAccountsService service)
        {
            var expectedRequest = new GetUserAccountsRequest(userId, email);
            apiClient.Setup(x =>
                    x.Get<GetUserAccountsResponse>(
                        It.Is<GetUserAccountsRequest>(c => c.GetUrl.Equals(expectedRequest.GetUrl))))
                .ReturnsAsync(new ApiResponse<GetUserAccountsResponse>(new GetUserAccountsResponse(), HttpStatusCode.InternalServerError, "Error"));

            var actual = await service.GetEmployerUserAccounts(email, userId);

            actual.Should().BeNull();
        }
    }
}