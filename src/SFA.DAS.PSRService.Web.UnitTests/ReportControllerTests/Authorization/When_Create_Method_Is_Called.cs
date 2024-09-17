﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using NUnit.Framework;
using SFA.DAS.PSRService.Web.Configuration.Authorization;

namespace SFA.DAS.PSRService.Web.UnitTests.ReportControllerTests.Authorization;

[Category("Made Obsolete by automation testing")]
[ExcludeFromCodeCoverage]
public sealed class WhenCreateMethodIsCalled : Given_A_ReportController
{
    private Attribute _attribute;

    protected override void When()
    {
        _attribute = Sut.GetType()
            .GetMethod(nameof(Sut.Create))
            .GetCustomAttribute(typeof(AuthorizeAttribute));
    }

    [Test]
    public void Then_Has_Authorization()
    {
        _attribute.Should().NotBeNull();
    }

    [Test]
    public void Then_Authorized_With_CanEdit_Policy()
    {
        ((AuthorizeAttribute)_attribute).Policy.Should().Be(PolicyNames.CanEditReport);
    }
}