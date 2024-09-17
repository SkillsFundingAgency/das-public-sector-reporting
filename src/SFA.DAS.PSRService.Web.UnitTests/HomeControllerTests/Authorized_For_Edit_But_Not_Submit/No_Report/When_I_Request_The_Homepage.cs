using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SFA.DAS.PSRService.Web.DisplayText;
using SFA.DAS.PSRService.Web.ViewModels.Home;

namespace SFA.DAS.PSRService.Web.UnitTests.HomeControllerTests.Authorized_For_Edit_But_Not_Submit.No_Report;

[TestFixture]
public class When_I_Request_The_Homepage : And_No_Current_Report_Exists
{ private IActionResult result;
    private ViewResult viewResult;
    private IndexViewModel model;

    protected override void When()
    {
        result = Sut.Index();

        viewResult = result as ViewResult;
        model = viewResult?.Model as IndexViewModel;
    }

    [Test]
    public void Then_ViewResult_Is_Returned()
    {
        viewResult = result as ViewResult;
    }
    [Test]
    public void Then_ViewResult_Is_No_Null()
    {
        viewResult.Should().NotBeNull();
    }
    [Test]
    public void Then_Model_Is_An_IndexViewModel()
    {
        model = viewResult.Model as IndexViewModel;

    }
    [Test]
    public void Then_Model_Is_Not_Null()
    {
        model.Should().NotBeNull();
    }

    [Test]
    public void Then_Create_Report_Is_Enabled()
    {
        model.CanCreateReport.Should().BeTrue();
    }

    [Test]
    public void Then_Edit_Report_Is_Disabled()
    {
        model.CanEditReport.Should().BeFalse();
    }

    [Test]
    public void Then_Readonly_Is_False()
    {
        model.Readonly.Should().BeFalse();
    }

    [Test]
    public void Then_CurrentReportAlreadySubmitted_Is_False()
    {
        model.CurrentReportAlreadySubmitted.Should().BeFalse();
    }

    [Test]
    public void Then_The_Welcome_Message_Is_Edit_No_Report()
    {
        var
            expectedMessage
                =
                HomePageWelcomeMessageProvider
                    .GetMesssage()
                    .ForPeriod(CurrentPeriod)
                    .WhereUserCanEdit()
                    .AndReportDoesNotExist();

        model
            .WelcomeMessage
            .Should()
            .BeEquivalentTo(
                expectedMessage);
    }
}