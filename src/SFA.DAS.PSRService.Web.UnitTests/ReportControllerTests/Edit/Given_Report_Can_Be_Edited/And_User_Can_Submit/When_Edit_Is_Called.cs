using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SFA.DAS.PSRService.Web.ViewModels;

namespace SFA.DAS.PSRService.Web.UnitTests.ReportControllerTests.Edit.Given_Report_Can_Be_Edited.And_User_Can_Submit;

public sealed class When_Edit_Is_Called
    :And_User_Can_Submit
{
    private IActionResult result;
    private ReportViewModel model;

    protected override void When()
    {
        base.When();

        result = Sut.Edit();

        var viewResult = result as ViewResult;

        model = viewResult?.Model as ReportViewModel;
    }
    [Test]
    public void Then_ViewModel_UserCanSubmitReports_Is_True()
    {
        model
            .UserCanSubmitReports
            .Should()
            .BeTrue();
    }

    [Test]
    public void Then_Result_Is_ViewResult()
    {
        Assert
            .IsNotNull(result);

        Assert
            .IsInstanceOf<ViewResult>(result);
    }

    [Test]
    public void Then_ViewName_Is_Edit()
    {
        Assert
            .AreEqual("Edit", ((ViewResult)result).ViewName, "View name does not match, should be: Summary");
    }

    [Test]
    public void Then_ViewModel_Is_ReportViewModel()
    {
        Assert
            .IsNotNull(((ViewResult)result).Model);

        Assert
            .IsInstanceOf<ReportViewModel>(((ViewResult)result).Model);
    }
}