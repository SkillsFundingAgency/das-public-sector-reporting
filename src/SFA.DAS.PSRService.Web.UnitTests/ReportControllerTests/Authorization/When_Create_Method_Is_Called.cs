﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using NUnit.Framework;
using SFA.DAS.PSRService.Web.Configuration.Authorization;
using SFA.DAS.PSRService.Web.Controllers;

namespace SFA.DAS.PSRService.Web.UnitTests.ReportControllerTests.Authorization;

[Category("Made Obsolete by automation testing")]
[ExcludeFromCodeCoverage]
public sealed class When_Create_Method_Is_Called : Given_A_ReportController
{
    //TODO: Delete when automation testing completed
    private Attribute attribute;

    protected override void When()
    {
        attribute = SUT.GetType()
            .GetMethod(nameof(SUT.Create))
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
    public void Then_Authorized_With_CanEdit_Policy()
    {
        Assert
            .AreEqual(
                ((AuthorizeAttribute)attribute).Policy,
                PolicyNames.CanEditReport);
    }
}