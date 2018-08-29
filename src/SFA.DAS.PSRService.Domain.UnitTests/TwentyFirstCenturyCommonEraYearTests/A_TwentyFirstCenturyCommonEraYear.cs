using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Domain.UnitTests.TwentyFirstCenturyCommonEraYearTests
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class A_TwentyFirstCenturyCommonEraYear
    {
        [TestCase("00", 2000)]
        [TestCase("99", 2099)]
        [TestCase("12", 2012)]
        [TestCase("19", 2019)]
        public void Parses_Correctly_From_Two_Digit_Year(
            string twoDigitYear,
            int expectedParsedYear)
        {
            TwentyFirstCenturyCommonEraYear
                .ParseTwoDigitYear(twoDigitYear)
                .AsInt
                .Should()
                .Be(expectedParsedYear);
        }

        [TestCase("00")]
        [TestCase("99")]
        [TestCase("12")]
        [TestCase("19")]
        public void Has_Expected_AsTwoDigitString_Property(
            string twoDigitYear)
        {
            TwentyFirstCenturyCommonEraYear
                .ParseTwoDigitYear(twoDigitYear)
                .AsTwoDigitString
                .Should()
                .Be(twoDigitYear);
        }

        [TestCase("")]
        [TestCase("    ")]
        [TestCase("1")]
        [TestCase("123")]
        [TestCase("-1")]
        [TestCase("1a")]
        [TestCase("a1")]
        public void Throws_ArgumentException_When_Asked_To_Parse_Invalid_Two_Digit_Year(
            string invalidTwoDigitYear)
        {
            Assert
                .That(
                    () => TwentyFirstCenturyCommonEraYear.ParseTwoDigitYear(invalidTwoDigitYear),
                    Throws
                        .Exception
                        .TypeOf<ArgumentException>());
        }
    }
}