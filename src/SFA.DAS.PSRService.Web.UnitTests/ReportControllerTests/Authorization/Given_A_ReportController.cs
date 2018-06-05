using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Moq;
using NUnit.Framework;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Web.Configuration.Authorization;
using SFA.DAS.PSRService.Web.Controllers;
using SFA.DAS.PSRService.Web.Services;

namespace SFA.DAS.PSRService.Web.UnitTests.ReportControllerTests.Authorization
{
    [ExcludeFromCodeCoverage]
    public abstract class Given_A_ReportController : GivenWhenThen<ReportController>
    {
        private Mock<IPeriodService> _periodServiceMock;
        private Mock<IAuthorizationService> _authorizationServiceMock;
        private Attribute attribute;

        protected override void Given()
        {
            _authorizationServiceMock = new Mock<IAuthorizationService>(MockBehavior.Strict);
            _periodServiceMock = new Mock<IPeriodService>();

            _periodServiceMock.Setup(s => s.GetCurrentPeriod()).Returns(new Period(DateTime.UtcNow));

            SUT = new ReportController(null,null,null,null,_periodServiceMock.Object,_authorizationServiceMock.Object);
        }

      

      
    }
}