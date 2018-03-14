﻿using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using SFA.DAS.AssessorService.Application.Api.Middleware;
using SFA.DAS.PSRService.Web.Controllers;
using Assert = NUnit.Framework.Assert;

namespace SFA.DAS.PSRService.Web.UnitTests.HomeControllerTests
{
    [TestFixture]
    public class WhenHttpRequestIsMadeAnd
    {
        private ErrorLoggingMiddleware _errorLoggingMiddleware;
        private ErrorLoggingMiddleware _errorLoggingMiddlewareException;
        private Mock<RequestDelegate> _requestDelegateMock;
        private Mock<ILogger<ErrorLoggingMiddleware>> _loggingMock;

        [SetUp]
        public void SetUp()
        {
            _loggingMock = new Mock<ILogger<ErrorLoggingMiddleware>>(MockBehavior.Strict);
           

            

            _errorLoggingMiddleware = new ErrorLoggingMiddleware(next: async (innerHttpContext) =>
            {
                await innerHttpContext.Response.WriteAsync("test response body");
            }, logger: _loggingMock.Object);


             _errorLoggingMiddlewareException = new ErrorLoggingMiddleware(next: (innerHttpContext) => throw new Exception("Error Logging Middleware Exception Raised"), logger: _loggingMock.Object);

        }

        [Test]
        public void NoErrorIsRaisedThenOk()
        {
            // arrange
            
            // act
            var result = _errorLoggingMiddleware.InvokeAsync(new DefaultHttpContext());

            // assert
          _loggingMock.VerifyAll();

           

            
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsCompleted);
            Assert.IsFalse(result.IsFaulted);
        }


        [Test]
        [ExpectedException(typeof(Exception), "No Error Logging Middleware Exception Raised")]
        public void ErrorIsRaisedThenExceptionReturned()
        {
            // arrange

            // act
            var result = _errorLoggingMiddlewareException.InvokeAsync(new DefaultHttpContext());

            // assert
            _loggingMock.VerifyAll();

            

            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsCompleted);
            Assert.IsTrue(result.IsFaulted);
        }

    }
}
