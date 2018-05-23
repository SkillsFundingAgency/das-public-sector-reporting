using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.PSRService.Web.Configuration.Authorization;
using SFA.DAS.PSRService.Web.ViewModels;

namespace SFA.DAS.PSRService.Web.UnitTests.ReportControllerTests.Summary
{
    [ExcludeFromCodeCoverage]
    public class And_User_Is_Not_Authorized_To_Submit_Report
        : Given_A_Valid_Report
    {
        public And_User_Is_Not_Authorized_To_Submit_Report()
        {
            MockAuthorizationService
                .Setup(
                    m => m.AuthorizeAsync(
                        It.IsAny<ClaimsPrincipal>(),
                        It.IsAny<object>(),
                        PolicyNames.CanSubmitReport))
                .Returns(
                    Task.FromResult(AuthorizationResult.Failed()));
        }

        [ExcludeFromCodeCoverage]
        [TestFixture]
        public class When_Summary_Is_Called
            : And_User_Is_Not_Authorized_To_Submit_Report
        {
            private IActionResult result;

            public When_Summary_Is_Called()
            {
                result = _controller.Summary("1718");
            }

            [Test]
            public void Then_ViewModel_UserCanSubmitReports_Is_False()
            {
                var reportViewModel = ((ViewResult) result).Model as ReportViewModel;

                Assert
                    .IsFalse(reportViewModel.UserCanSubmitReports);
            }
        }
    }
}