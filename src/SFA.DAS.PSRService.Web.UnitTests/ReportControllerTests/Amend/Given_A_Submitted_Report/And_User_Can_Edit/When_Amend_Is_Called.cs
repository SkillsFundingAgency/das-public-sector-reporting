using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
using SFA.DAS.PSRService.Application.ReportHandlers;

namespace SFA.DAS.PSRService.Web.UnitTests.ReportControllerTests.Amend.Given_A_Submitted_Report.And_User_Can_Edit
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class When_Amend_Is_Called
        : And_User_Can_Edit
    {
        private IActionResult result;
        private string ExpectedUrl = @"Report/Edit";

        protected override void Given()
        {
            base.Given();
            
            MockUrlHelper
                .Setup(
                    m =>
                        m.Action(
                            It.Is<UrlActionContext>(
                                ctx =>
                                    ctx.Action.Equals("Edit", StringComparison.OrdinalIgnoreCase)
                                    && ctx.Controller.Equals("Report", StringComparison.OrdinalIgnoreCase))
                        ))
                .Returns(
                    ExpectedUrl);

        }

        protected override void When()
        {
            base.When();

            result = _controller.Amend();
        }

        [Test]
        public void Then_UnSubmit_Command_Is_Called()
        {
            MockMediatr
                .Verify(
                    m =>
                        m.Send(It.Is<UnSubmitReportRequest>(r => r.Report.Id == CurrentReport.Id),
                            It.IsAny<CancellationToken>())
                );
        }

        [Test]
        public void Then_I_Am_Redirected_To_EditPage()
        {
            result
                .Should()
                .BeAssignableTo<RedirectResult>();

            ((RedirectResult) result)
                .Url
                .Should()
                .BeEquivalentTo(ExpectedUrl);
        }
    }
}