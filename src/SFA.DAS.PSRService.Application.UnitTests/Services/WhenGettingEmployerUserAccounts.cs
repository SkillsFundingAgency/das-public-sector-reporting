using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.GovUK.Auth.Employer;
using SFA.DAS.PSRService.Application.EmployerUserAccounts.Requests;
using SFA.DAS.PSRService.Application.EmployerUserAccounts.Responses;
using SFA.DAS.PSRService.Application.OuterApi;
using SFA.DAS.PSRService.Application.Services;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.PSRService.Application.UnitTests.Services;

public class WhenGettingEmployerUserAccounts
{
    [Test, MoqAutoData]
    public async Task Then_The_Outer_Api_Is_Called_And_Accounts_Returned(
        string email,
        string userId,
        GetUserAccountsResponse apiResponse,
        [Frozen] Mock<IOuterApiClient> outerApiClient,
        UserAccountService service)
    {
        //Arrange
        var apiResponseWrapper = new ApiResponse<GetUserAccountsResponse>(apiResponse, System.Net.HttpStatusCode.OK, null);
        var request = new GetUserAccountsRequest(userId, email);
        outerApiClient
            .Setup(x => x.Get<GetUserAccountsResponse>(
                It.Is<GetUserAccountsRequest>(c => c.GetUrl.Equals(request.GetUrl)))).ReturnsAsync(apiResponseWrapper);
        
        //Act
        var actual = await ((IGovAuthEmployerAccountService)service).GetUserAccounts(userId, email);

        //Assert
        actual.Should().BeEquivalentTo(new GovUK.Auth.Employer.EmployerUserAccounts
        {
            EmployerAccounts = apiResponse.UserAccounts?.Select(c => new EmployerUserAccountItem
            {
                Role = c.Role,
                AccountId = c.AccountId,
                EmployerName = c.EmployerName,
                ApprenticeshipEmployerType = c.ApprenticeshipEmployerType,
            }).ToList() ?? [],
            FirstName = apiResponse.FirstName,
            IsSuspended = apiResponse.IsSuspended,
            LastName = apiResponse.LastName,
            EmployerUserId = apiResponse.EmployerUserId,
        });
    }
}