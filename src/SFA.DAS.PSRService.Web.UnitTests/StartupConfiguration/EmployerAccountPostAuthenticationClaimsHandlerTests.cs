using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using AutoFixture.NUnit3;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SFA.DAS.GovUK.Auth.Employer;
using SFA.DAS.PSRService.Web.Configuration;
using EmployerAccountPostAuthenticationClaimsHandler = SFA.DAS.PSRService.Web.StartupConfiguration.EmployerAccountPostAuthenticationClaimsHandler;

namespace SFA.DAS.PSRService.Web.UnitTests.StartupConfiguration;

public class EmployerAccountPostAuthenticationClaimsHandlerTests
{
    [Test, AutoData]
    public async Task Then_The_Claims_Are_Populated_For_Gov_User(string nameIdentifier, string emailAddress, EmployerUserAccounts accountData)
    {
        // Arrange
        accountData.IsSuspended = false;

        var accountService = new Mock<IGovAuthEmployerAccountService>();
        var handler = new EmployerAccountPostAuthenticationClaimsHandler(accountService.Object);

        var tokenValidatedContext = ArrangeTokenValidatedContext(nameIdentifier, emailAddress);
        accountService
            .Setup(x => x.GetUserAccounts(nameIdentifier, emailAddress)).ReturnsAsync(accountData)
            .Verifiable();
        
        // Act
        var actual = await handler.GetClaims(tokenValidatedContext);

        // Assert
        accountService.Verify();
        accountService.VerifyNoOtherCalls();
        actual.Should().ContainSingle(c => c.Type.Equals(EmployerPsrsClaims.AccountsClaimsTypeIdentifier));

        var actualClaimValue = actual.First(c => c.Type.Equals(EmployerPsrsClaims.AccountsClaimsTypeIdentifier)).Value;
        JsonConvert.SerializeObject(accountData.EmployerAccounts.ToDictionary(k => k.AccountId)).Should().Be(actualClaimValue);

        actual.First(c => c.Type.Equals(EmployerPsrsClaims.IdamsUserIdClaimTypeIdentifier)).Value.Should().Be(accountData.EmployerUserId);
        actual.First(c => c.Type.Equals(EmployerPsrsClaims.EmailClaimsTypeIdentifier)).Value.Should().Be(emailAddress);
        actual.First(c => c.Type.Equals(EmployerPsrsClaims.NameClaimsTypeIdentifier)).Value.Should().Be($"{accountData.FirstName} {accountData.LastName}");
        actual.FirstOrDefault(c => c.Type.Equals(ClaimTypes.AuthorizationDecision)).Should().BeNull();
    }

    [Test, AutoData]
    public async Task Then_The_Claims_Are_Populated_For_Gov_User_Thats_Suspended(string nameIdentifier, string emailAddress, EmployerUserAccounts accountData)
    {
        accountData.IsSuspended = true;

        var accountService = new Mock<IGovAuthEmployerAccountService>();
        var handler = new EmployerAccountPostAuthenticationClaimsHandler(accountService.Object);

        var tokenValidatedContext = ArrangeTokenValidatedContext(nameIdentifier, emailAddress);
        accountService.Setup(x => x.GetUserAccounts(nameIdentifier, emailAddress)).ReturnsAsync(accountData);

        var actual = await handler.GetClaims(tokenValidatedContext);

        accountService.Verify(x => x.GetUserAccounts(nameIdentifier, emailAddress), Times.Once);
        actual.Should().ContainSingle(c => c.Type.Equals(EmployerPsrsClaims.AccountsClaimsTypeIdentifier));

        var actualClaimValue = actual.First(c => c.Type.Equals(EmployerPsrsClaims.AccountsClaimsTypeIdentifier)).Value;
        JsonConvert.SerializeObject(accountData.EmployerAccounts.ToDictionary(k => k.AccountId)).Should().Be(actualClaimValue);

        actual.First(c => c.Type.Equals(EmployerPsrsClaims.IdamsUserIdClaimTypeIdentifier)).Value.Should().Be(accountData.EmployerUserId);
        actual.First(c => c.Type.Equals(EmployerPsrsClaims.EmailClaimsTypeIdentifier)).Value.Should().Be(emailAddress);
        actual.First(c => c.Type.Equals(EmployerPsrsClaims.NameClaimsTypeIdentifier)).Value.Should().Be($"{accountData.FirstName} {accountData.LastName}");
        actual.First(c => c.Type.Equals(ClaimTypes.AuthorizationDecision)).Value.Should().Be("Suspended");
    }

    private static TokenValidatedContext ArrangeTokenValidatedContext(string nameIdentifier, string emailAddress)
    {
        var identity = new ClaimsIdentity(new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, nameIdentifier),
            new(ClaimTypes.Email, emailAddress)
        });

        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(identity));

        return new TokenValidatedContext(new DefaultHttpContext(), new AuthenticationScheme(",", "", typeof(TestAuthHandler)),
            new OpenIdConnectOptions(), Mock.Of<ClaimsPrincipal>(), new AuthenticationProperties())
        {
            Principal = claimsPrincipal
        };
    }

    private class TestAuthHandler : IAuthenticationHandler
    {
        public Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
        {
            throw new NotImplementedException();
        }

        public Task<AuthenticateResult> AuthenticateAsync()
        {
            throw new NotImplementedException();
        }

        public Task ChallengeAsync(AuthenticationProperties properties)
        {
            throw new NotImplementedException();
        }

        public Task ForbidAsync(AuthenticationProperties properties)
        {
            throw new NotImplementedException();
        }
    }
}