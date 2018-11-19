using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.PSRService.Messages.Events;

namespace SFA.DAS.PSRService.Application.UnitTests.MappingTests.Given_A_Report_To_ReportUpdated_Mapper.And_A_Valid_Report_Source
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public sealed class When_Mapped
    : And_A_Valid_Report_Source
    {
        private ReportUpdated mapped;

        protected override void When()
        {
            mapped = SUT.Map<ReportUpdated>(SourceReport);
        }

        [Test]
        public void Then_Mapped_ReportedUpdate_Event_Has_Correct_Values()
        {
            reportUpdatedEventContainsCorrectDataForSourceReport(mapped);
            
        }
        private void reportUpdatedEventContainsCorrectDataForSourceReport(ReportUpdated message)
        {
            message
                .Id
                .Should()
                .Be(SourceReport.Id);

            message
                .ReportingPeriod
                .Should()
                .Be(SourceReport.Period.PeriodString);

            message
                .EmployerId
                .Should()
                .Be(SourceReport.EmployerId);

            verifyAnswers(message.Answers);

            verifyReportingPercentages(message.ReportingPercentages);
        }

        private void verifyAnswers(Answers messageAnswers)
        {
            verifyReportNumbersAnswers(messageAnswers);
            verifyFactorsAnswers(messageAnswers);
        }

        private void verifyFactorsAnswers(Answers messageAnswers)
        {
            messageAnswers
                .OutlineActions
                .Should()
                .Be(
                    SourceReport
                        .Answers
                        .OutlineActions);

            messageAnswers
                .Challenges
                .Should()
                .Be(
                    SourceReport
                        .Answers
                        .Challenges);

            messageAnswers
                .TargetPlans
                .Should()
                .Be(
                    SourceReport
                        .Answers
                        .TargetPlans);

            messageAnswers
                .AnythingElse
                .Should()
                .Be(
                    SourceReport
                        .Answers
                        .AnythingElse);
        }

        private void verifyReportingPercentages(Messages.Events.ReportingPercentages messageReportingPercentages)
        {
            messageReportingPercentages
                .EmploymentStarts
                .Should()
                .Be(
                    SourceReport
                        .ReportingPercentages
                        .EmploymentStarts);

            messageReportingPercentages
                .NewThisPeriod
                .Should()
                .Be(
                    SourceReport
                        .ReportingPercentages
                        .NewThisPeriod);

            messageReportingPercentages
                .TotalHeadCount
                .Should()
                .Be(
                    SourceReport
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
                    SourceReport
                        .Answers
                        .FullTimeEquivalents);
        }

        private void verifyYourApprenticesAnswers(YourApprentices yourApprenticesAnswers)
        {
            yourApprenticesAnswers
                .AtStart
                .Should()
                .Be(
                    SourceReport
                        .Answers
                        .YourApprentices
                        .AtStart);
            yourApprenticesAnswers
                .AtEnd
                .Should()
                .Be(
                    SourceReport
                        .Answers
                        .YourApprentices
                        .AtEnd);
            yourApprenticesAnswers
                .NewThisPeriod
                .Should()
                .Be(
                    SourceReport
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
                    SourceReport
                        .Answers
                        .YourEmployees
                        .AtStart);

            yourEmployeesAnswers
                .AtEnd
                .Should()
                .Be(
                    SourceReport
                        .Answers
                        .YourEmployees
                        .AtEnd);

            yourEmployeesAnswers
                .NewThisPeriod
                .Should()
                .Be(
                    SourceReport
                        .Answers
                        .YourEmployees
                        .NewThisPeriod);
        }
    }
}