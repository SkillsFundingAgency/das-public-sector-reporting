using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Domain.UnitTests.Domain.UserTests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public sealed class When_Setting_Name_Property
    {
        private User SUT;

        [SetUp]
        public void CreateSUT()
        {
            SUT = new User();
        }

        [Test]
        public void To_Null_Then_Exception_Is_Thrown()
        {
            Assert.Catch<ArgumentException>(() => SUT.Name = null);
        }

        [Test]
        public void To_Empty_String_Then_Exception_Is_Thrown()
        {
            Assert.Catch<ArgumentException>(() => SUT.Name = String.Empty);
        }

        [Test]
        public void To_Whitespace_String_Then_Exception_Is_Thrown()
        {
            Assert.Catch<ArgumentException>(() => SUT.Name = "      ");
        }

        [Test]
        public void To_NonEmpty_NonWhitespace_String_Then_Property_Is_Set()
        {
            string validName = "UncleTom";

            SUT.Name = validName;

            SUT.Name
                .Should()
                .BeEquivalentTo(
                    validName);
        }
    }
}