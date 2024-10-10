using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.PSRService.Domain.Values;

namespace SFA.DAS.PSRService.Domain.UnitTests.TwoThousandsCommonEraYearTests;

[TestFixture]
[ExcludeFromCodeCoverage]
public class A_TwoThousandsCommonEraYear
{
    [TestCase("00", "00", true)]
    [TestCase("00", "10", false)]
    [TestCase("99", "99", true)]
    [TestCase("00", "98", false)]
    [TestCase("01", "20", false)]
    public void Is_Equatable_With_Another_TwoThousandsCommonEraYear(string left, string right, bool expectedEqual)
    {
        var leftYear = TwoThousandsCommonEraYear.ParseTwoDigitYear(left);
        var rightYear = TwoThousandsCommonEraYear.ParseTwoDigitYear(right);

        leftYear
            .Equals(rightYear)
            .Should()
            .Be(expectedEqual);
    }

    [TestCase(2000)]
    [TestCase(2001)]
    [TestCase(2019)]
    [TestCase(2099)]
    public void Can_Be_Created_From_Integer_Year(int year)
    {
        TwoThousandsCommonEraYear
            .FromYearAsNumber(year)
            .Should()
            .NotBeNull();
    }

    [TestCase(3000)]
    [TestCase(200)]
    [TestCase(3019)]
    [TestCase(1999)]
    [TestCase(9999)]
    [TestCase(2999)]
    [TestCase(2100)]
    public void Throws_Exception_When_Trying_To_Create_From_Invalid_Integer_Year(int invalidYear)
    {
        var action = () => TwoThousandsCommonEraYear.FromYearAsNumber(invalidYear);
        action.Should().Throw<ArgumentException>();
    }

    [TestCase("00", 2000)]
    [TestCase("99", 2099)]
    [TestCase("12", 2012)]
    [TestCase("19", 2019)]
    public void Parses_Correctly_From_Two_Digit_Year(string twoDigitYear, int expectedParsedYear) {
        TwoThousandsCommonEraYear
            .ParseTwoDigitYear(twoDigitYear)
            .AsInt
            .Should()
            .Be(expectedParsedYear);
    }

    [TestCase("00")]
    [TestCase("99")]
    [TestCase("12")]
    [TestCase("19")]
    public void Has_Expected_AsTwoDigitString_Property(string twoDigitYear)
    {
        TwoThousandsCommonEraYear
            .ParseTwoDigitYear(twoDigitYear)
            .AsTwoDigitString
            .Should()
            .Be(twoDigitYear);
    }

    [TestCase("00", "2000")]
    [TestCase("99", "2099")]
    [TestCase("12", "2012")]
    [TestCase("19", "2019")]
    public void Has_Expected_AsFourDigitString_Property(string twoDigitYear, string expectedFourDigitYear)
    {
        TwoThousandsCommonEraYear
            .ParseTwoDigitYear(twoDigitYear)
            .AsFourDigitString
            .Should()
            .Be(expectedFourDigitYear);
    }

    [TestCase("")]
    [TestCase("    ")]
    [TestCase("1")]
    [TestCase("123")]
    [TestCase("-1")]
    [TestCase("1a")]
    [TestCase("a1")]
    public void Throws_ArgumentException_When_Asked_To_Parse_Invalid_Two_Digit_Year(string invalidTwoDigitYear)
    {
        var action = () => TwoThousandsCommonEraYear.ParseTwoDigitYear(invalidTwoDigitYear);
        action.Should().Throw<ArgumentException>();
    }
}