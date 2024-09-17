using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Moq;
using SFA.DAS.PSRService.Web.Configuration.Authorization;

namespace SFA.DAS.PSRService.Web.UnitTests.ReportControllerTests.Summary.Given_A_Valid_Not_Submitted_Report.And_User_Has_View_Only_Access;

[ExcludeFromCodeCoverage]
public abstract class And_User_Has_View_Only_Access : GivenAValidSubmittedReport
{
    protected override void Given()
    {
        base.Given();

        MockAuthorizationService.Setup(m => m.AuthorizeAsync(
                It.IsAny<ClaimsPrincipal>(),
                It.IsAny<object>(),
                PolicyNames.CanSubmitReport))
            .Returns(Task.FromResult(AuthorizationResult.Failed()));

        MockAuthorizationService.Setup(m => m.AuthorizeAsync(
                It.IsAny<ClaimsPrincipal>(),
                It.IsAny<object>(),
                PolicyNames.CanEditReport))
            .Returns(Task.FromResult(AuthorizationResult.Failed()));
    }
}