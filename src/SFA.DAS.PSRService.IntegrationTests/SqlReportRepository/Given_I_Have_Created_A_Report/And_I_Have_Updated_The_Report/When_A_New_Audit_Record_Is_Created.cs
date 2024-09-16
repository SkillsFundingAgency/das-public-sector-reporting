using System;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using SFA.DAS.PSRService.Application.Domain;

namespace SFA.DAS.PSRService.IntegrationTests.SqlReportRepository.Given_I_Have_Created_A_Report.And_I_Have_Updated_The_Report;

[TestFixture]
[ExcludeFromCodeCoverage]
public class When_A_New_Audit_Record_Is_Created : And_I_Have_Updated_The_Report_Data
{
    private AuditRecordDto _auditRecordDto;

    protected override void When()
    {
        _auditRecordDto = new AuditRecordDto
        {
            ReportId = CreatedReport.Id,
            ReportingData = "Trump Dossier",
            UpdatedBy = "Sergey & Oleg",
            UpdatedUtc = RepositoryTestHelper.TrimDateTime(DateTime.UtcNow)
        };

        SUT.SaveAuditRecord(_auditRecordDto);
    }

    [Test]
    public void Then_Audit_Record_Appears_In_Database()
    {
        var list = SUT.GetAuditRecordsMostRecentFirst(_auditRecordDto.ReportId);
        Assert.AreEqual(1, list.Count);
        var actualRecord = list[0];
        Assert.AreEqual(_auditRecordDto.ReportId, actualRecord.ReportId);
        Assert.AreEqual(_auditRecordDto.ReportingData, actualRecord.ReportingData);
        Assert.AreEqual(_auditRecordDto.UpdatedBy, actualRecord.UpdatedBy);
        Assert.AreEqual(_auditRecordDto.UpdatedUtc, actualRecord.UpdatedUtc);
    }
}