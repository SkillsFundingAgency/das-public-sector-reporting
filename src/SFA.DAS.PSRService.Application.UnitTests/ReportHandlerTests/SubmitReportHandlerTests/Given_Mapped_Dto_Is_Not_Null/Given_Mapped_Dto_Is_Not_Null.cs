using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using AutoMapper;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.NServiceBus;
using SFA.DAS.PSRService.Application.Domain;
using SFA.DAS.PSRService.Application.Interfaces;
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

        private ReportDto _mappedDto;
        private Mock<IEventPublisher> _mockEventPublisher;
        private Report _reportToBeSubmitted;

        protected override void Given()
        {
            _mockRepository = new Mock<IReportRepository>();
            _mockEventPublisher = new Mock<IEventPublisher>();

            var mockMapper = new Mock<IMapper>();

            _mappedDto = new ReportDto();

            _mappedDto.Submitted = false;

            _mappedDto.Id = new Guid("33F2BAD1-F368-4467-A249-D2B936284458");

            mockMapper
                .Setup(
                    m => m.Map<ReportDto>(It.IsAny<Report>())
                )
                .Returns(_mappedDto);

            SUT = new SubmitReportHandler(
                mockMapper.Object,
                _mockRepository.Object);
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
        public void Then_RepositoryUpdate_Is_Called_With_Mapped_Dto()
        {
            _mockRepository
                .Verify(
                    m => m.Update(
                        It.Is<ReportDto>( r => ReferenceEquals(r, _mappedDto))));
        }

        [Test]
        public void Then_Dto_Passed_To_RepositoryUpdate_Has_Submitted_Status()
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
        public void Then_RepositoryDeleteHistory_Is_Called_With_Id_Of_Mapped_Dto()
        {
            _mockRepository
                .Verify(
                    m => m.DeleteHistory(
                        It.Is<Guid>( g => g.Equals(_mappedDto.Id))));
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

        private void verifyReportingPercentages(ReportingPercentages messageReportingPercentages)
        {
            Assert.Fail("ReportSubmitted event reporting percentages not verified.");
        }

        private void verifyAnswers(Answers messageAnswers)
        {
            Assert.Fail("ReportSubmitted event answer not verified.");
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