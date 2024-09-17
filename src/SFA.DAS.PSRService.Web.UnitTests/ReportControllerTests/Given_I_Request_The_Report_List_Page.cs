using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Web.ViewModels;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.PSRService.Web.UnitTests.ReportControllerTests;

[TestFixture]
public class Given_I_Request_The_Report_List_Page : ReportControllerTestBase
{
    [Test, MoqAutoData]
    public async Task And_More_Than_One_Report_Then_Show_List(string hashedAccountId)
    {
        // arrange
        MockReportService.Setup(s => s.GetSubmittedReports(It.IsAny<string>())).ReturnsAsync(ReportList);
        
        // act
        var result = await Controller.List(hashedAccountId);

        // assert
        result.Should().BeOfType<ViewResult>();
        var listViewResult = result as ViewResult;
        listViewResult.Should().NotBeNull();
        listViewResult.ViewName.Should().Be("List", "View name does not match, should be: List");


        var listViewModel = listViewResult.Model as ReportListViewModel;
        listViewModel.Should().NotBeNull();

        var reportList = listViewModel.SubmittedReports;
        reportList.Should().NotBeEmpty();
        reportList.Should().AllBeOfType<Report>();
        reportList.Should().BeEquivalentTo(ReportList);
    }

    [Test, MoqAutoData]
    [Ignore("No longer a requirement")]
    public async Task And_Only_One_Report_Then_Redirect_To_Edit(string hashedAccountId)
    {
        // arrange
        const string url = "report/edit";
        UrlActionContext actualContext = null;

        MockUrlHelper.Setup(h => h.Action(It.IsAny<UrlActionContext>())).Returns(url).Callback<UrlActionContext>(c => actualContext = c).Verifiable("Url.Action was never called");

        MockReportService.Setup(s => s.GetSubmittedReports(It.IsAny<string>())).ReturnsAsync(ReportList.Take(1).ToList);
        // act
        var result = await Controller.List(hashedAccountId);

        // assert
        MockUrlHelper.VerifyAll();

        result.Should().BeOfType<RedirectResult>();
        var redirectResult = result as RedirectResult;
        redirectResult.Should().NotBeNull();
        redirectResult.Url.Should().Be(url);
        actualContext.Action.Should().Be("Edit");
        actualContext.Controller.Should().BeNull();
    }

    [Test, MoqAutoData]
    [Ignore("No longer a requirement")]
    public async Task And_No_Report_Then_Redirect_To_Start(string hashedAccountId)
    {
        const string url = "home/index";
        UrlActionContext actualContext = null;

        MockUrlHelper.Setup(h => h.Action(It.IsAny<UrlActionContext>())).Returns(url).Callback<UrlActionContext>(c => actualContext = c).Verifiable("Url.Action was never called");

        MockReportService.Setup(s => s.GetSubmittedReports(It.IsAny<string>())).ReturnsAsync(new List<Report>());
        // act
        var result = await Controller.List(hashedAccountId);

        // assert
        result.Should().BeOfType<RedirectResult>();
        var redirectResult = result as RedirectResult;
        redirectResult.Should().NotBeNull();
        redirectResult.Url.Should().Be(url);
        actualContext.Action.Should().Be("Index");
        actualContext.Controller.Should().Be("Home");
    }
}