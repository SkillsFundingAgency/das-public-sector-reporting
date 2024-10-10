using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using SFA.DAS.PSRService.Application.Domain;
using SFA.DAS.PSRService.Web.Configuration.Authorization;
using SFA.DAS.PSRService.Web.Controllers;
using SFA.DAS.PSRService.Web.ViewModels.Home;

namespace SFA.DAS.PSRService.IntegrationTests.Web;

[TestFixture]
public class WhenUserEntersSite
{
    private static IHost _host;
    private HomeController _homeController;

    [SetUp]
    public void SetUp()
    {
        TestHelper.ClearData();
        _homeController = _host.Services.GetService<HomeController>();
    }

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        var builder = Host.CreateApplicationBuilder();
        builder.Services.ConfigureTestServices();

        //Replace authorization service with one tailored to this test
        builder.Services.AddSingleton(BuildMockAuthorizationServiceWhereUserCanEditAndCanSubmit());
        _host = builder.Build();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _host?.Dispose();
    }

    [TearDown]
    public void TearDown()
    {
        _homeController?.Dispose();
    }

    private static IAuthorizationService BuildMockAuthorizationServiceWhereUserCanEditAndCanSubmit()
    {
        var policyNames = new[] { PolicyNames.CanEditReport, PolicyNames.CanSubmitReport };

        var service = new Mock<IAuthorizationService>();
        service.Setup(m => m.AuthorizeAsync(
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<object>(),
                    It.Is<string>(x => policyNames.Contains(x))))
            .Returns(Task.FromResult(AuthorizationResult.Success()));

        return service.Object;
    }

    [Test]
    public async Task AndThereAreNoReportsThenCreateReportIsEnabled()
    {
        // arrange

        // act
        var result = await _homeController.Index() as ViewResult;

        // assert
        result.Should().NotBeNull();
        result.Model.Should().BeOfType<IndexViewModel>();
        var model = (IndexViewModel)result.Model;
        model.CanCreateReport.Should().BeTrue();
        model.CanEditReport.Should().BeFalse();
    }

    [Test]
    public async Task AndThereIsSubmittedReportThenCreateReportIsDisabled()
    {
        // arrange
        TestHelper.CreateReport(new ReportDto
        {
            Id = Guid.NewGuid(),
            EmployerId = "111",
            Submitted = true,
            ReportingPeriod = TestHelper.CurrentPeriod.PeriodString,
            ReportingData = "{OrganisationName: '1'}"
        });

        // act
        var result = await _homeController.Index() as ViewResult;

        // assert
        result.Should().NotBeNull();
        result.Model.Should().BeOfType<IndexViewModel>();
        var model = (IndexViewModel)result.Model;
        model.CanCreateReport.Should().BeFalse();
        model.CanEditReport.Should().BeFalse();
    }

    [Test]
    public async Task AndThereIsUnsubmittedReportThenEditReportIsEnabled()
    {
        // arrange
        TestHelper.CreateReport(new ReportDto
        {
            Id = Guid.NewGuid(),
            EmployerId = "111",
            Submitted = false,
            ReportingPeriod = TestHelper.CurrentPeriod.PeriodString,
            ReportingData = "{OrganisationName: '1'}"
        });

        // act
        var result = await _homeController.Index() as ViewResult;

        // assert
        result.Should().NotBeNull();
        result.Model.Should().BeOfType<IndexViewModel>();
        var model = (IndexViewModel)result.Model;
        model.CanCreateReport.Should().BeFalse();
        model.CanEditReport.Should().BeTrue();
    }
}