using System;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.PSRService.Web.Middleware;

namespace SFA.DAS.PSRService.Web.UnitTests.ErrorLoggingMiddlewareTests;

[TestFixture]
public class WhenHttpRequestIsMadeAnd
{
    private ErrorLoggingMiddleware _errorLoggingMiddleware;
    private ErrorLoggingMiddleware _errorLoggingMiddlewareException;
    private Mock<ILogger<ErrorLoggingMiddleware>> _loggingMock;

    [SetUp]
    public void SetUp()
    {
        _loggingMock = new Mock<ILogger<ErrorLoggingMiddleware>>(MockBehavior.Strict);
            
        _errorLoggingMiddleware = new ErrorLoggingMiddleware(next: async (innerHttpContext) =>
        {
            await innerHttpContext.Response.WriteAsync("test response body");
        }, logger: _loggingMock.Object);

        _errorLoggingMiddlewareException = new ErrorLoggingMiddleware(next: _ => throw new Exception("Error Logging Middleware Exception Raised"), logger: _loggingMock.Object);
    }

    [Test]
    public void NoErrorIsRaisedThenOk()
    {
        // act
        var result = _errorLoggingMiddleware.InvokeAsync(new DefaultHttpContext());

        // assert
        _loggingMock.VerifyAll();
        
        result.Should().NotBeNull();
        result.IsCompleted.Should().BeTrue();
        result.IsFaulted.Should().BeFalse();
    }

    [Test]
    public void ErrorIsRaisedThenExceptionReturned()
    {
        // arrange

        // act
        var action = () => _errorLoggingMiddlewareException.InvokeAsync(new DefaultHttpContext());
        action.Should().ThrowAsync<AggregateException>();

        // assert
        _loggingMock.VerifyAll();
    }
}