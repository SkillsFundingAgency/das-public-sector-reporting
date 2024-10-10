using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using NUnit.Framework;

namespace SFA.DAS.PSRService.Domain.UnitTests.Domain.User;

[ExcludeFromCodeCoverage]
[TestFixture]
public sealed class When_Setting_Name_Property
{
    private Entities.User _sut;

    [SetUp]
    public void Setup() => _sut = new Entities.User();

    [Test]
    public void To_Null_Then_Exception_Is_Thrown()
    {
        var action = () => _sut.Name = null;
        action.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void To_Empty_String_Then_Exception_Is_Thrown()
    {
        var action = () => _sut.Name = string.Empty;
        action.Should().Throw<ArgumentException>();
    }

    [Test]
    public void To_Whitespace_String_Then_Exception_Is_Thrown()
    {
        var action = () => _sut.Name = "      ";
        action.Should().Throw<ArgumentException>();
    }

    [Test]
    public void To_NonEmpty_NonWhitespace_String_Then_Property_Is_Set()
    {
        const string validName = "UncleTom";

        _sut.Name = validName;

        _sut.Name.Should().BeEquivalentTo(validName);
    }
}