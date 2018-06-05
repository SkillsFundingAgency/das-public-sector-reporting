using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Moq;
using NUnit.Framework;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Configuration.Authorization;
using SFA.DAS.PSRService.Web.Controllers;
using SFA.DAS.PSRService.Web.Models;
using SFA.DAS.PSRService.Web.Services;

namespace SFA.DAS.PSRService.Web.UnitTests.HomeControllerTests.Is_Unauthorized
{
    public class And_Is_Unauthorized : Given_Home_Controller
    {
      
        protected override void Given()
        {
            base.Given();
            _authorizationServiceMock.Setup(m => m.AuthorizeAsync(
                                                It.IsAny<ClaimsPrincipal>(),
                                                It.IsAny<object>(),
                                                PolicyNames.CanEditReport))
                                                .Returns(Task.FromResult(AuthorizationResult.Failed()));
        }
    }
}
