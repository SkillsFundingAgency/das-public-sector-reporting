using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using AutoMapper;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.NServiceBus;
using SFA.DAS.PSRService.Application.Domain;
using SFA.DAS.PSRService.Application.Interfaces;
using SFA.DAS.PSRService.Application.Mapping;
using SFA.DAS.PSRService.Application.ReportHandlers;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Messages.Events;
using ReportingPercentages = SFA.DAS.PSRService.Messages.Events.ReportingPercentages;

namespace SFA.DAS.PSRService.Application.UnitTests.ReportHandlerTests.SubmitReportHandlerTests.Given_Mapped_Dto_Is_Not_Null
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class Given_Mapped_Dto_Is_Not_Null
        : GivenWhenThen<SubmitReportHandler>
    {
        private Mock<IReportRepository> _mockRepository;

        private Mock<IEventPublisher> _mockEventPublisher;
        private Report _reportToBeSubmitted;

        protected override void Given()
        {
            _mockRepository = new Mock<IReportRepository>();
            _mockEventPublisher = new Mock<IEventPublisher>();

            var config = new MapperConfiguration(
                cfg =>
                {
                    cfg.AddProfile<ReportMappingProfile>();
                    cfg.AddProfile<ReportSubmittedProfile>();
                });

            SUT = new SubmitReportHandler(
                config.CreateMapper(),
                _mockRepository.Object,
                _mockEventPublisher.Object);
        }

        protected override void When()
        {
            _reportToBeSubmitted = new Report();
            SUT
                .Handle(
                    new SubmitReportRequest(_reportToBeSubmitted),
                    new CancellationToken());
        }

        [Test]
        public void Then_RepositoryUpdate_Is_Called_With_Submitted_Report_Id()
        {
            _mockRepository
                .Verify(
                    m => m.Update(
                        It.Is<ReportDto>( r => r.Id.Equals(_reportToBeSubmitted.Id))));
        }

        [Test]
        public void Then_RepositoryUpdate_Is_Called_With_Correct_Submitted_Status()
        {
           _mockRepository
               .Verify(
                   m => m.Update(
                       It.Is<ReportDto>( r => r.Submitted == true)));
        }

        [Test]
        public void Then_RepositoryDeleteHistory_Is_Called()
        {
            _mockRepository
                .Verify(
                    m => m.DeleteHistory(
                        It.IsAny<Guid>()), Times.Once);
        }

        [Test]
        public void Then_RepositoryDeleteHistory_Is_Called_With_Id_Of_Submitted_Report()
        {
            _mockRepository
                .Verify(
                    m => m.DeleteHistory(
                        It.Is<Guid>( g => g.Equals(_reportToBeSubmitted.Id))));
        }

        [Test]
        public void Then_A_Correct_ReportSubmitted_Event_Is_Published()
        {
            _mockEventPublisher
                .Verify(
                    m => m.Publish(
                        It.Is<ReportSubmitted>(message =>
                            reportSubmittedEventContainsCorrectDataForSubmitReportRequest(message)
                        )));
        }

        private bool reportSubmittedEventContainsCorrectDataForSubmitReportRequest(ReportSubmitted message)
        {
            message
                .Id
                .Should()
                .Be(_reportToBeSubmitted.Id);

            message
                .ReportingPeriod
                .Should()
                .Be(_reportToBeSubmitted.Period.PeriodString);

            message
                .EmployerId
                .Should()
                .Be(_reportToBeSubmitted.EmployerId);

            verifyReportSubmitter(message.Submitter);

            verifyAnswers(message.Answers);

            verifyReportingPercentages(message.ReportingPercentages);

            return true;
        }

        private void verifyAnswers(Answers messageAnswers)
        {
            verifyReportNumbersAnswers(messageAnswers);
            verifyFactorsAnswers(messageAnswers);

            Assert.Fail("ReportSubmitted event answer not verified.");
        }

        private void verifyFactorsAnswers(Answers messageAnswers)
        {
            messageAnswers
                .OutlineActions
                .Should()
                .Be(
                    _reportToBeSubmitted
                        .Answers
                        .OutlineActions);

            messageAnswers
                .Challenges
                .Should()
                .Be(
                    _reportToBeSubmitted
                        .Answers
                        .Challenges);

            messageAnswers
                .TargetPlans
                .Should()
                .Be(
                    _reportToBeSubmitted
                        .Answers
                        .TargetPlans);

            messageAnswers
                .AnythingElse
                .Should()
                .Be(
                    _reportToBeSubmitted
                        .Answers
                        .AnythingElse);
        }

        private void verifyReportingPercentages(ReportingPercentages messageReportingPercentages)
        {
            messageReportingPercentages
                .EmploymentStarts
                .Should()
                .Be(
                    _reportToBeSubmitted
                        .ReportingPercentages
                        .EmploymentStarts);

            messageReportingPercentages
                .NewThisPeriod
                .Should()
                .Be(
                    _reportToBeSubmitted
                        .ReportingPercentages
                        .NewThisPeriod);

            messageReportingPercentages
                .TotalHeadCount
                .Should()
                .Be(
                    _reportToBeSubmitted
                        .ReportingPercentages
                        .TotalHeadCount);
        }

        private void verifyReportNumbersAnswers(Answers messageAnswers)
        {
            verifyYourEmployeesAnswers(messageAnswers.YourEmployees);
            verifyYourApprenticesAnswers(messageAnswers.YourApprentices);

            messageAnswers
                .FullTimeEquivalents
                .Should()
                .Be(
                    _reportToBeSubmitted
                        .Answers
                        .FullTimeEquivalents);
        }

        private void verifyYourApprenticesAnswers(YourApprentices yourApprenticesAnswers)
        {
            yourApprenticesAnswers
                .AtStart
                .Should()
                .Be(
                    _reportToBeSubmitted
                        .Answers
                        .YourApprentices
                        .AtStart);
            yourApprenticesAnswers
                .AtEnd
                .Should()
                .Be(
                    _reportToBeSubmitted
                        .Answers
                        .YourApprentices
                        .AtEnd);
            yourApprenticesAnswers
                .NewThisPeriod
                .Should()
                .Be(
                    _reportToBeSubmitted
                        .Answers
                        .YourApprentices
                        .NewThisPeriod);
        }

        private void verifyYourEmployeesAnswers(YourEmployees yourEmployeesAnswers)
        {
            yourEmployeesAnswers
                .AtStart
                .Should()
                .Be(
                    _reportToBeSubmitted
                        .Answers
                        .YourEmployees
                        .AtStart);

            yourEmployeesAnswers
                .AtEnd
                .Should()
                .Be(
                    _reportToBeSubmitted
                        .Answers
                        .YourEmployees
                        .AtEnd);

            yourEmployeesAnswers
                .NewThisPeriod
                .Should()
                .Be(
                    _reportToBeSubmitted
                        .Answers
                        .YourEmployees
                        .NewThisPeriod);
        }

        private void verifyReportSubmitter(Submitter messageSubmitter)
        {
            messageSubmitter
                .Email
                .Should()
                .Be(_reportToBeSubmitted.SubmittedDetails.SubmittedEmail);

            messageSubmitter
                .Name
                .Should()
                .Be(_reportToBeSubmitted.SubmittedDetails.SubmittedName);

            messageSubmitter
                .UserId
                .Should()
                .Be(_reportToBeSubmitted.SubmittedDetails.SubmttedBy);
        }
    }
}