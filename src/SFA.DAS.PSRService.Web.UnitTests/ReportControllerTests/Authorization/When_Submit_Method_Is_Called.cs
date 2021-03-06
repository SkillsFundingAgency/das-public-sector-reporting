﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using NUnit.Framework;
using SFA.DAS.PSRService.Web.Configuration.Authorization;
using SFA.DAS.PSRService.Web.Controllers;

namespace SFA.DAS.PSRService.Web.UnitTests.ReportControllerTests.Authorization
{
    [ExcludeFromCodeCoverage]
    [Category("Made Obsolete by automation testing")]
    public sealed class When_Submit_Method_Is_Called : Given_A_ReportController
    {
        
        private Attribute attribute;

        protected override void When()
        {
            attribute = SUT.GetType()
                .GetMethod(nameof(SUT.SubmitPost))
                .GetCustomAttribute(typeof(AuthorizeAttribute));
        }

        [Test]
        public void Then_Has_Authorization()
        {
            Assert
                .NotNull(
                    attribute);
        }

        [Test]
        public void Then_Authorized_With_CanSubmit_Policy()
        {
            Assert
                .AreEqual(
                    ((AuthorizeAttribute)attribute).Policy,
                    PolicyNames.CanSubmitReport);
        }
    }
}