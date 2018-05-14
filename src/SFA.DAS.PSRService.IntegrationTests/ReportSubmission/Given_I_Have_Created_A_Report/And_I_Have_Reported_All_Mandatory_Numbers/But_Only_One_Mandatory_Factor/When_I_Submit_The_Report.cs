using FluentAssertions;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;

namespace SFA.DAS.PSRService.IntegrationTests.ReportSubmission.Given_I_Have_Created_A_Report.And_I_Have_Reported_All_Mandatory_Numbers.But_Only_One_Mandatory_Factor
{
    public sealed class When_I_Submit_The_Report
    : But_Only_One_Mandatory_Factor
    {
        private IActionResult submitResult;

        protected override void When()
        {
            submitResult
                =
                SUT
                    .Submit(
                        TestHelper
                            .CurrentPeriod
                            .PeriodString);
        }

        [Test]
        public void Then_I_Am_Redirected_To_The_Home_Index()
        {
            Assert
                .IsInstanceOf<RedirectResult>(
                    submitResult);

            MockUrlHelper
                .Verify(
                    m =>
                        m.Action(
                            It.Is<UrlActionContext>(
                                ctx
                                    =>
                                        ctx.Action.Equals("Index", StringComparison.OrdinalIgnoreCase))));

            MockUrlHelper
                .Verify(
                    m =>
                        m.Action(
                            It.Is<UrlActionContext>(
                                ctx
                                    =>
                                        ctx.Controller.Equals("Home", StringComparison.OrdinalIgnoreCase))));
        }

        [Test]
        public void Then_Report_Is_Not_Persisted_As_Submitted()
        {
            TestHelper
                .GetAllReports()
                .Should()
                .NotContain(
                    report => report.Submitted == true);
        }
    }
}