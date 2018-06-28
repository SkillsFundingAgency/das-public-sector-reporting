using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace SFA.DAS.PSRService.Web.UnitTests.ReportControllerTests.OrganisationName.Given_A_Submitted_Report
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public sealed class When_I_Call_OrganisationName
        : Given_Report_Cannot_Be_Edited
    {
        private IActionResult result;

        protected override void When()
        {
            base.When();

            result = SUT.OrganisationName(string.Empty);
        }

        [Test]
        public void Then_I_Am_Redirected_To_HomePage()
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