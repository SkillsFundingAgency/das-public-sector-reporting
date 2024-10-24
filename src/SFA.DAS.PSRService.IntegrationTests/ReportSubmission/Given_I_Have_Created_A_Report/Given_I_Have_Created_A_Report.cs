using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using SFA.DAS.PSRService.IntegrationTests.Web;
using SFA.DAS.PSRService.Web.Controllers;
using SFA.DAS.PSRService.Web.ViewModels;

namespace SFA.DAS.PSRService.IntegrationTests.ReportSubmission.Given_I_Have_Created_A_Report;

[ExcludeFromCodeCoverage]
public abstract class Given_I_Have_Created_A_Report(bool isLocalAuthority) : GivenWhenThen<ReportController>
{
    protected QuestionController QuestionController;
    private Mock<IUrlHelper> _mockUrlHelper;
    private IHost _host;

    protected override async Task Given()
    {
        var builder = Host.CreateApplicationBuilder();
        builder.Services.ConfigureTestServices();
        _host = builder.Build();
        
        _mockUrlHelper = new Mock<IUrlHelper>();
        _mockUrlHelper.Setup(u => u.Action(It.IsAny<UrlActionContext>())).Returns("!");

        QuestionController = _host.Services.GetService<QuestionController>();

        QuestionController.Url = _mockUrlHelper.Object;

        Sut = _host.Services.GetService<ReportController>();
        Sut.Url = _mockUrlHelper.Object;
        Sut.ObjectValidator = Mock.Of<IObjectModelValidator>();

        var mockContext = new Mock<HttpContext>();
        mockContext.Setup(c => c.User).Returns(new TestPrincipal());

        Sut.ControllerContext.HttpContext = mockContext.Object;
        QuestionController.ControllerContext.HttpContext = mockContext.Object;
        TestHelper.ClearData();

        await Sut.PostIsLocalAuthority(new IsLocalAuthorityViewModel() { IsLocalAuthority = isLocalAuthority });
    }

    [TearDown]
    public void TearDown()
    {
        QuestionController?.Dispose();
        _host?.Dispose();
    }
}