using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Moq;
using NUnit.Framework;
using SFA.DAS.PSRService.Application.ReportHandlers;

namespace SFA.DAS.PSRService.Web.UnitTests.ServiceTests.ReportServiceTests.Submit.Given_A_Valid_Report
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class When_Submit_Is_Called
    : And_Report_Is_For_This_Period
    {
        public When_Submit_Is_Called()
        {
            SUT
                .SubmitReport(
                    ValidNotSubmittedReport);
        }

        [Test]
        public void Then_SubmitReportRequest_Is_Sent_To_Mediator()
        {
            MockMediator
                .Verify(
                    m => m.Send(
                        It.IsAny<SubmitReportRequest>(),
                        It.IsAny<CancellationToken>()));
        }
    }
}