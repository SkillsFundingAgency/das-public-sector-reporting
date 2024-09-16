﻿using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using NUnit.Framework;

namespace SFA.DAS.PSRService.Web.UnitTests.Middleware.RoleRequierments.Given_User_Does_Not_Have_Claim;

[ExcludeFromCodeCoverage]
public sealed class When_Requirement_Is_Handled : Given_User_Does_Not_Have_Claim
{
    protected override void When()
    {
        Sut.HandleAsync(HandlerContext);
    }

    [Test]
    public void Then_Requirement_Is_Not_Satisfied()
    {
        HandlerContext.HasSucceeded.Should().BeFalse();
    }
}