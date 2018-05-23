using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using NUnit.Framework;
using SFA.DAS.PSRService.Web.Configuration.Authorization;
using SFA.DAS.PSRService.Web.Controllers;

namespace SFA.DAS.PSRService.Web.UnitTests.ReportControllerTests.Authorization
{
    [ExcludeFromCodeCoverage]
    public class Given_A_ReportController
    {
        private ReportController SUT;
        private Attribute attribute;

        public Given_A_ReportController()
        {
            attribute = typeof(ReportController)
                .GetMethod(nameof(ReportController.Submit))
                .GetCustomAttribute(typeof(AuthorizeAttribute));
        }

        [Test]
        public void Then_Submit_Method_Has_Authorization()
        {
            Assert
                .NotNull(
                    attribute);
        }

        [Test]
        public void Then_Submit_Method_Is_Authorized_With_CanSubmit_Policy()
        {
            Assert
                .AreEqual(
                    ((AuthorizeAttribute)attribute).Policy,
                    PolicyNames.CanSubmitReport);
        }
    }
}