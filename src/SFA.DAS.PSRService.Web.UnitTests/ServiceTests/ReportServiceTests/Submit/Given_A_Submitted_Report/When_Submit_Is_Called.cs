using System;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace SFA.DAS.PSRService.Web.UnitTests.ServiceTests.ReportServiceTests.Submit.Given_A_Submitted_Report;

[ExcludeFromCodeCoverage]
[TestFixture]
public class When_Submit_Is_Called
    : Given_A_Submitted_Report
{
    [Test]
    public void Then_InvalidOperationException_Is_Thrown()
    {
        Assert
            .That(
                () => Sut.SubmitReport(AlreadySubmittedReport),
                Throws
                    .Exception
                    .TypeOf<InvalidOperationException>());
    }
}