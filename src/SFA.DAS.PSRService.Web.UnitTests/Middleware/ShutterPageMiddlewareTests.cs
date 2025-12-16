using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Middleware;

namespace SFA.DAS.PSRService.Web.UnitTests.Middleware;

[TestFixture]
public class ShutterPageMiddlewareTests
{
    private Mock<RequestDelegate> _nextDelegateMock;
    private Mock<IWebConfiguration> _webConfigurationMock;
    private DefaultHttpContext _httpContext;
    private ShutterPageMiddleware _middleware;

    [SetUp]
    public void SetUp()
    {
        _nextDelegateMock = new Mock<RequestDelegate>();
        _webConfigurationMock = new Mock<IWebConfiguration>();
        _httpContext = new DefaultHttpContext();
        _middleware = new ShutterPageMiddleware(_nextDelegateMock.Object, _webConfigurationMock.Object);
    }

    [Test]
    public async Task InvokeAsync_WhenShutterPageDisabled_ShouldCallNextDelegate()
    {
        // Arrange
        _webConfigurationMock.Setup(c => c.ShutterPageEnabled).Returns(false);
        _httpContext.Request.Path = "/accounts/ABC123/home";

        // Act
        await _middleware.InvokeAsync(_httpContext);

        // Assert
        _nextDelegateMock.Verify(x => x(It.Is<HttpContext>(c => c == _httpContext)), Times.Once);
    }

    [Test]
    public async Task InvokeAsync_WhenShutterPageEnabledAndHealthCheckEndpoint_ShouldCallNextDelegate()
    {
        // Arrange
        _webConfigurationMock.Setup(c => c.ShutterPageEnabled).Returns(true);
        _httpContext.Request.Path = "/ping";

        // Act
        await _middleware.InvokeAsync(_httpContext);

        // Assert
        _nextDelegateMock.Verify(x => x(It.Is<HttpContext>(c => c == _httpContext)), Times.Once);
    }

    [Test]
    public async Task InvokeAsync_WhenShutterPageEnabledAndViewFound_ShouldRenderShutterPage()
    {
        // Arrange
        _webConfigurationMock.Setup(c => c.ShutterPageEnabled).Returns(true);
        _webConfigurationMock.Setup(c => c.EmployerAccountsBaseUrl).Returns("https://test.accounts.com");
        _httpContext.Request.Path = "/accounts/ABC123/home";
        _httpContext.Response.Body = new MemoryStream();

        var viewMock = new Mock<Microsoft.AspNetCore.Mvc.ViewEngines.IView>();
        viewMock.Setup(v => v.RenderAsync(It.IsAny<ViewContext>()))
            .Returns((ViewContext context) =>
            {
                var writer = context.Writer;
                return writer.WriteAsync("<html>Shutter Page</html>");
            });

        var viewEngineResult = ViewEngineResult.Found("Shared/Shutter", viewMock.Object);
        var razorViewEngineMock = new Mock<IRazorViewEngine>();
        razorViewEngineMock
            .Setup(e => e.FindView(It.IsAny<ActionContext>(), "Shared/Shutter", false))
            .Returns(viewEngineResult);

        var tempDataProviderMock = new Mock<ITempDataProvider>();

        var serviceProvider = new Mock<IServiceProvider>();
        serviceProvider.Setup(sp => sp.GetService(typeof(IRazorViewEngine))).Returns(razorViewEngineMock.Object);
        serviceProvider.Setup(sp => sp.GetService(typeof(ITempDataProvider))).Returns(tempDataProviderMock.Object);

        _httpContext.RequestServices = serviceProvider.Object;

        // Act
        await _middleware.InvokeAsync(_httpContext);

        // Assert
        _nextDelegateMock.Verify(x => x(It.IsAny<HttpContext>()), Times.Never);
        _httpContext.Response.StatusCode.Should().Be(200);
        _httpContext.Response.ContentType.Should().Be("text/html; charset=utf-8");
        viewMock.Verify(v => v.RenderAsync(It.IsAny<ViewContext>()), Times.Once);
    }

    [Test]
    public async Task InvokeAsync_WhenShutterPageEnabledAndViewNotFound_ShouldCallNextDelegate()
    {
        // Arrange
        _webConfigurationMock.Setup(c => c.ShutterPageEnabled).Returns(true);
        _httpContext.Request.Path = "/accounts/ABC123/home";

        var viewEngineResult = ViewEngineResult.NotFound("Shared/Shutter", new[] { "Shared/Shutter" });
        var razorViewEngineMock = new Mock<IRazorViewEngine>();
        razorViewEngineMock
            .Setup(e => e.FindView(It.IsAny<ActionContext>(), "Shared/Shutter", false))
            .Returns(viewEngineResult);

        var tempDataProviderMock = new Mock<ITempDataProvider>();

        var serviceProvider = new Mock<IServiceProvider>();
        serviceProvider.Setup(sp => sp.GetService(typeof(IRazorViewEngine))).Returns(razorViewEngineMock.Object);
        serviceProvider.Setup(sp => sp.GetService(typeof(ITempDataProvider))).Returns(tempDataProviderMock.Object);

        _httpContext.RequestServices = serviceProvider.Object;

        // Act
        await _middleware.InvokeAsync(_httpContext);

        // Assert
        _nextDelegateMock.Verify(x => x(It.Is<HttpContext>(c => c == _httpContext)), Times.Once);
    }

    [Test]
    public async Task InvokeAsync_WhenAccountIdInRouteData_ShouldExtractAndUseIt()
    {
        // Arrange
        _webConfigurationMock.Setup(c => c.ShutterPageEnabled).Returns(true);
        _webConfigurationMock.Setup(c => c.EmployerAccountsBaseUrl).Returns("https://test.accounts.com");
        _httpContext.Request.Path = "/accounts/ABC123/home";
        _httpContext.Response.Body = new MemoryStream();

        var routeData = new RouteData();
        routeData.Values["hashedEmployerAccountId"] = "ABC123";
        
        var routingFeature = new RoutingFeature { RouteData = routeData };
        _httpContext.Features.Set<IRoutingFeature>(routingFeature);

        var viewMock = new Mock<Microsoft.AspNetCore.Mvc.ViewEngines.IView>();
        viewMock.Setup(v => v.RenderAsync(It.IsAny<ViewContext>())).Returns(Task.CompletedTask);

        var viewEngineResult = ViewEngineResult.Found("Shared/Shutter", viewMock.Object);
        var razorViewEngineMock = new Mock<IRazorViewEngine>();
        razorViewEngineMock
            .Setup(e => e.FindView(It.IsAny<ActionContext>(), "Shared/Shutter", false))
            .Returns(viewEngineResult);

        var tempDataProviderMock = new Mock<ITempDataProvider>();

        var serviceProvider = new Mock<IServiceProvider>();
        serviceProvider.Setup(sp => sp.GetService(typeof(IRazorViewEngine))).Returns(razorViewEngineMock.Object);
        serviceProvider.Setup(sp => sp.GetService(typeof(ITempDataProvider))).Returns(tempDataProviderMock.Object);

        _httpContext.RequestServices = serviceProvider.Object;

        // Act
        await _middleware.InvokeAsync(_httpContext);

        // Assert
        razorViewEngineMock.Verify(e => e.FindView(
            It.Is<ActionContext>(ac => 
                ac.RouteData.Values.ContainsKey("hashedEmployerAccountId") &&
                ac.RouteData.Values["hashedEmployerAccountId"].ToString() == "ABC123"),
            "Shared/Shutter",
            false), Times.Once);
    }

    [Test]
    public async Task InvokeAsync_WhenAccountIdInPath_ShouldExtractFromPath()
    {
        // Arrange
        _webConfigurationMock.Setup(c => c.ShutterPageEnabled).Returns(true);
        _webConfigurationMock.Setup(c => c.EmployerAccountsBaseUrl).Returns("https://test.accounts.com");
        _httpContext.Request.Path = "/accounts/XYZ789/home";
        _httpContext.Response.Body = new MemoryStream();

        var viewMock = new Mock<Microsoft.AspNetCore.Mvc.ViewEngines.IView>();
        viewMock.Setup(v => v.RenderAsync(It.IsAny<ViewContext>())).Returns(Task.CompletedTask);

        var viewEngineResult = ViewEngineResult.Found("Shared/Shutter", viewMock.Object);
        var razorViewEngineMock = new Mock<IRazorViewEngine>();
        razorViewEngineMock
            .Setup(e => e.FindView(It.IsAny<ActionContext>(), "Shared/Shutter", false))
            .Returns(viewEngineResult);

        var tempDataProviderMock = new Mock<ITempDataProvider>();

        var serviceProvider = new Mock<IServiceProvider>();
        serviceProvider.Setup(sp => sp.GetService(typeof(IRazorViewEngine))).Returns(razorViewEngineMock.Object);
        serviceProvider.Setup(sp => sp.GetService(typeof(ITempDataProvider))).Returns(tempDataProviderMock.Object);

        _httpContext.RequestServices = serviceProvider.Object;

        // Act
        await _middleware.InvokeAsync(_httpContext);

        // Assert
        razorViewEngineMock.Verify(e => e.FindView(
            It.Is<ActionContext>(ac => 
                ac.RouteData.Values.ContainsKey("hashedEmployerAccountId") &&
                ac.RouteData.Values["hashedEmployerAccountId"].ToString() == "XYZ789"),
            "Shared/Shutter",
            false), Times.Once);
    }

    [Test]
    public async Task InvokeAsync_WhenAccountIdNotPresent_ShouldUseBaseUrlOnly()
    {
        // Arrange
        _webConfigurationMock.Setup(c => c.ShutterPageEnabled).Returns(true);
        _webConfigurationMock.Setup(c => c.EmployerAccountsBaseUrl).Returns("https://test.accounts.com");
        _httpContext.Request.Path = "/some/path";
        _httpContext.Response.Body = new MemoryStream();

        var viewMock = new Mock<Microsoft.AspNetCore.Mvc.ViewEngines.IView>();
        viewMock.Setup(v => v.RenderAsync(It.IsAny<ViewContext>())).Returns(Task.CompletedTask);

        var viewEngineResult = ViewEngineResult.Found("Shared/Shutter", viewMock.Object);
        var razorViewEngineMock = new Mock<IRazorViewEngine>();
        razorViewEngineMock
            .Setup(e => e.FindView(It.IsAny<ActionContext>(), "Shared/Shutter", false))
            .Returns(viewEngineResult);

        var tempDataProviderMock = new Mock<ITempDataProvider>();

        var serviceProvider = new Mock<IServiceProvider>();
        serviceProvider.Setup(sp => sp.GetService(typeof(IRazorViewEngine))).Returns(razorViewEngineMock.Object);
        serviceProvider.Setup(sp => sp.GetService(typeof(ITempDataProvider))).Returns(tempDataProviderMock.Object);

        _httpContext.RequestServices = serviceProvider.Object;

        // Act
        await _middleware.InvokeAsync(_httpContext);

        // Assert
        razorViewEngineMock.Verify(e => e.FindView(
            It.Is<ActionContext>(ac => 
                !ac.RouteData.Values.ContainsKey("hashedEmployerAccountId")),
            "Shared/Shutter",
            false), Times.Once);
    }

    [Test]
    public async Task InvokeAsync_WhenEmployerAccountsBaseUrlIsEmpty_ShouldUseEmptyHomeUrl()
    {
        // Arrange
        _webConfigurationMock.Setup(c => c.ShutterPageEnabled).Returns(true);
        _webConfigurationMock.Setup(c => c.EmployerAccountsBaseUrl).Returns("");
        _httpContext.Request.Path = "/accounts/ABC123/home";
        _httpContext.Response.Body = new MemoryStream();

        var viewMock = new Mock<Microsoft.AspNetCore.Mvc.ViewEngines.IView>();
        viewMock.Setup(v => v.RenderAsync(It.IsAny<ViewContext>())).Returns(Task.CompletedTask);

        var viewEngineResult = ViewEngineResult.Found("Shared/Shutter", viewMock.Object);
        var razorViewEngineMock = new Mock<IRazorViewEngine>();
        razorViewEngineMock
            .Setup(e => e.FindView(It.IsAny<ActionContext>(), "Shared/Shutter", false))
            .Returns(viewEngineResult);

        var tempDataProviderMock = new Mock<ITempDataProvider>();

        var serviceProvider = new Mock<IServiceProvider>();
        serviceProvider.Setup(sp => sp.GetService(typeof(IRazorViewEngine))).Returns(razorViewEngineMock.Object);
        serviceProvider.Setup(sp => sp.GetService(typeof(ITempDataProvider))).Returns(tempDataProviderMock.Object);

        _httpContext.RequestServices = serviceProvider.Object;

        // Act
        await _middleware.InvokeAsync(_httpContext);

        // Assert
        _httpContext.Response.StatusCode.Should().Be(200);
        viewMock.Verify(v => v.RenderAsync(It.IsAny<ViewContext>()), Times.Once);
    }

    private class RoutingFeature : IRoutingFeature
    {
        public RouteData RouteData { get; set; } = new();
    }
}
