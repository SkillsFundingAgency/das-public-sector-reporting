using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.PSRService.Application.EmployerUserAccounts;
using SFA.DAS.PSRService.Application.EmployerUserAccounts.Responses;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.StartupConfiguration;

namespace SFA.DAS.PSRService.Web.UnitTests.StartupConfiguration
{
    public class EmployerAccountPostAuthenticationClaimsHandlerTests
    {
        [Test, AutoData]
        public async Task Then_The_Claims_Are_Populated_For_Gov_User(
            string nameIdentifier,
            string emailAddress,
            GetUserAccountsResponse accountData
            )
        {
            var accountService = new Mock<IEmployerUserAccountsService>();
            var handler =
                new EmployerAccountPostAuthenticationClaimsHandler(accountService.Object);
            
            var tokenValidatedContext = ArrangeTokenValidatedContext(nameIdentifier, emailAddress);
            accountService.Setup(x => x.GetEmployerUserAccounts(emailAddress, nameIdentifier)).ReturnsAsync(accountData);
        
            var actual = await handler.GetClaims(tokenValidatedContext);
        
            accountService.Verify(x=>x.GetEmployerUserAccounts(emailAddress,nameIdentifier), Times.Once);
            actual.Should().ContainSingle(c => c.Type.Equals(EmployerPsrsClaims.AccountsClaimsTypeIdentifier));
            var actualClaimValue = actual.First(c => c.Type.Equals(EmployerPsrsClaims.AccountsClaimsTypeIdentifier)).Value;
            JsonConvert.SerializeObject(accountData.UserAccounts.ToDictionary(k => k.AccountId)).Should().Be(actualClaimValue);
            actual.First(c => c.Type.Equals(EmployerPsrsClaims.IdamsUserIdClaimTypeIdentifier)).Value.Should().Be(accountData.EmployerUserId);
            actual.First(c => c.Type.Equals(EmployerPsrsClaims.EmailClaimsTypeIdentifier)).Value.Should().Be(emailAddress);
            actual.First(c => c.Type.Equals(EmployerPsrsClaims.NameClaimsTypeIdentifier)).Value.Should().Be($"{accountData.FirstName} {accountData.LastName}");
        }
        
        private TokenValidatedContext ArrangeTokenValidatedContext(string nameIdentifier, string emailAddress)
        {
            var identity = new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, nameIdentifier),
                
                new Claim(ClaimTypes.Email, emailAddress)
            });
        
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(identity));
            return new TokenValidatedContext(new DefaultHttpContext(), new AuthenticationScheme(",","", typeof(TestAuthHandler)),
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
}