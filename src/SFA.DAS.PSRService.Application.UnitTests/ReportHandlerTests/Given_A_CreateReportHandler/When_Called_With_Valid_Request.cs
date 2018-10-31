using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.PSRService.Application.Domain;
using SFA.DAS.PSRService.Application.ReportHandlers;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Messages.Events;

namespace SFA.DAS.PSRService.Application.UnitTests.ReportHandlerTests.Given_A_CreateReportHandler
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public sealed class When_Called_With_Valid_Request
    : Given_A_CreateReportHandler
    {
        private Report _response;
        private Guid _userId;
        private string _userName;
        private CreateReportRequest _createReportRequest;

        protected override void When()
        {
            _userId = Guid.NewGuid();
            _userName = "Bob Shurunkle";

            ReportDto reportDto = null;

            _createReportRequest = new CreateReportRequestBuilder()
                .WithUserName(_userName)
                .WithUserId(_userId)
                .WithEmployerId(EmployerId)
                .ForPeriod(ReportingPeriod)
                .Build();

            _response = SUT.Handle(_createReportRequest, new CancellationToken()).Result;
        }

        [Test]
        public void Then_Created_Report_EmployerId_Is_Same_As_Request_EmployerId()
        {
            _response
                .EmployerId
                .Should()
                .Be(_createReportRequest.EmployerId);
        }

        [Test]
        public void Then_Created_Report_SubmittedStatus_Is_False()
        {
            _response
                .Submitted
                .Should()
                .BeFalse();
        }

        [Test]
        public void Then_Created_Report_ReportingPeriod_Is_Same_As_Request_ReportingPeriod()
        {
            _response
                .ReportingPeriod
                .Should()
                .Be(_createReportRequest.Period.PeriodString);
        }

        [Test]
        public void Then_Repository_Is_Called_With_Same_EmployerId_As_Request()
        {
            ReportRepositoryMock
                .Verify(
                    m => m.Create(
                        It.Is<ReportDto>(
                            arg => arg.EmployerId.Equals(_createReportRequest.EmployerId))));
        }

        [Test]
        public void Then_Repository_Is_Called_With_Same_Period_As_Request()
        {
            ReportRepositoryMock
                .Verify(
                    m => m.Create(
                        It.Is<ReportDto>(
                            arg => arg.ReportingPeriod.Equals(_createReportRequest.Period.PeriodString))));
        }

        [Test]
        public void Then_Repository_Is_Called_With_Not_Null_AuditWindowStartUtc()
        {
            ReportRepositoryMock
                .Verify(
                    m => m.Create(
                        It.Is<ReportDto>(
                            arg => arg.AuditWindowStartUtc != null)));
        }

        [Test]
        public void Then_Repository_Is_Called_With_Not_Null_UpdatedUtc()
        {
            ReportRepositoryMock
                .Verify(
                    m => m.Create(
                        It.Is<ReportDto>(
                            arg => arg.UpdatedUtc != null)));
        }

        [Test]
        public void Then_A_Valid_ReportCreated_Message_Is_Published()
        {
            MockEventPublisher
                .Verify(
                    m => m.Publish(
                        It.Is<ReportCreated>( message => validateReportCreatedMessage(message))));
        }

        private bool validateReportCreatedMessage(ReportCreated message)
        {
            message
                .EmployerId
                .Should()
                .Be(_createReportRequest.EmployerId, "Should have same EmployerId as create report request.");

            message
                .ReportingPeriod
                .Should()
                .Be(_createReportRequest.Period.PeriodString, "should have same reporting period as create report request.");

            message
                .Id
                .Should()
                .Be(_response.Id, "should have same id as created report.");

            return true;
        }

        [Test]
        public void Then_Repository_Is_Called_With_A_Valid_UpdatedBy()
        {
            ReportRepositoryMock
                .Verify(
                    m => m.Create(
                        It.Is<ReportDto>(
                            arg => validateUpdatedBy(arg.UpdatedBy))));
        }

        private bool validateUpdatedBy(string updatedBy)
        {
            var user = JsonConvert.DeserializeObject<User>(updatedBy);

            user
                .Id
                .Should()
                .Be(_userId);

            user
                .Name
                .Should()
                .Be(_userName);

            return true;
        }
    }
}