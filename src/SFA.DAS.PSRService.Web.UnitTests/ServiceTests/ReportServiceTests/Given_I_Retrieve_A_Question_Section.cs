using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using SFA.DAS.PSRService.Application.ReportHandlers;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Domain.Enums;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Controllers;
using SFA.DAS.PSRService.Web.Services;
using Assert = NUnit.Framework.Assert;
using CollectionAssert = NUnit.Framework.CollectionAssert;

namespace SFA.DAS.PSRService.Web.UnitTests.ReportControllerTests
{
    [TestFixture]
    public class Given_I_Retrieve_A_Question_Section
    {
        public Mock<IMediator> _mediatorMock;
        public Mock<IWebConfiguration> _webConfiguration;
        public ReportService _reportService;

        [SetUp]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
            _webConfiguration = new Mock<IWebConfiguration>(MockBehavior.Strict);

            _reportService = new ReportService(_webConfiguration.Object,_mediatorMock.Object);

        }
        
        [Test]
        public void And_The_Question_Section_Exists_Then_Return_Section()
        {
            // arrange
            var Questions = new List<Question>()
            {
                new Question()
                {
                    Id = "atStart",
                    Answer = "0",
                    Type = QuestionType.Number,
                    Optional = false
                }
                ,new Question()
                {
                    Id = "atEnd",
                    Answer = "0",
                    Type = QuestionType.Number,
                    Optional = false
                },
                new Question()
                {
                    Id = "newThisPeriod",
                    Answer = "0",
                    Type = QuestionType.Number,
                    Optional = false
                }

            };

            var SectionOne = new Section()
            {
                Id = "SectionOne",
                SubSections = new List<Section>() { new Section{
                    Id = "SubSectionOne",
                    Questions = Questions,
                    Title = "SubSectionOne",
                    SummaryText = ""

                }},
                Questions = null,
                Title = "SectionOne"
            };

            var SectionTwo = new Section()
            {
                Id = "SectionTwo",
                SubSections = new List<Section>() { new Section{
                    Id = "SubSectionTwo",
                    Questions = Questions,
                    Title = "SubSectionTwo",
                    SummaryText = ""

                }},
                Questions = null,
                Title = "SectionTwo"
            };

            var SectionThree = new Section()
            {
                Id = "SectionThree",
                SubSections = new List<Section>() { new Section{
                    Id = "SubSectionThree",
                    Questions = Questions,
                    Title = "SubSectionThree",
                    SummaryText = ""

                }},
                Questions = null,
                Title = "SectionThree"
            };

            IList<Section> sections = new List<Section>();

            sections.Add(SectionOne);
            sections.Add(SectionTwo);
            sections.Add(SectionThree);
            var report = new Report()
            {
                Sections = sections
            };
            // act
            var result = _reportService.GetQuestionSection("SubSectionTwo", report);

        
            Assert.NotNull(result);
         

            Assert.AreEqual(result.Id, "SubSectionTwo");
            Assert.AreEqual(result.Title, "SubSectionTwo");
            Assert.NotNull(result.Questions);
            CollectionAssert.IsNotEmpty(result.Questions);

        }

        [Test]
     
        public void And_The_Report_Contains_No_Sections_Then_Throw_Error()
        {
            Assert.Throws<Exception>(() => _reportService.GetQuestionSection("SubSectionTwo", new Report()));
            
        }

        [Test]
        public void And_The_Question_Section_Exists_More_Than_Once_Then_Throw_Error()
        {
            // arrange
            var Questions = new List<Question>()
            {
                new Question()
                {
                    Id = "atStart",
                    Answer = "0",
                    Type = QuestionType.Number,
                    Optional = false
                }
                ,new Question()
                {
                    Id = "atEnd",
                    Answer = "0",
                    Type = QuestionType.Number,
                    Optional = false
                },
                new Question()
                {
                    Id = "newThisPeriod",
                    Answer = "0",
                    Type = QuestionType.Number,
                    Optional = false
                }

            };

            var SectionOne = new Section()
            {
                Id = "SectionOne",
                SubSections = new List<Section>() { new Section{
                    Id = "SubSectionOne",
                    Questions = Questions,
                    Title = "SubSectionOne",
                    SummaryText = ""

                }},
                Questions = null,
                Title = "SectionOne"
            };

            var SectionTwo = new Section()
            {
                Id = "SectionTwo",
                SubSections = new List<Section>() { new Section{
                    Id = "SubSectionOne",
                    Questions = Questions,
                    Title = "SubSectionTwo",
                    SummaryText = ""

                }},
                Questions = null,
                Title = "SectionTwo"
            };

            var SectionThree = new Section()
            {
                Id = "SectionThree",
                SubSections = new List<Section>() { new Section{
                    Id = "SubSectionThree",
                    Questions = Questions,
                    Title = "SubSectionThree",
                    SummaryText = ""

                }},
                Questions = null,
                Title = "SectionThree"
            };

            IList<Section> sections = new List<Section>();

            sections.Add(SectionOne);
            sections.Add(SectionTwo);
            sections.Add(SectionThree);
            var report = new Report()
            {
                Sections = sections
            };
            // act

            Assert.Throws<InvalidOperationException>(() => _reportService.GetQuestionSection("SectionOne", report));


        }
    }
}
