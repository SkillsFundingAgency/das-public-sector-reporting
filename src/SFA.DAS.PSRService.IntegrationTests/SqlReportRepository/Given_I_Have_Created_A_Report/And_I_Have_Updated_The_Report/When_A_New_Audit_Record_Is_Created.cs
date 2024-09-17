using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
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
        list.Count.Should().Be(1);
        var actualRecord = list[0];
        actualRecord.ReportId.Should().Be(_auditRecordDto.ReportId);
        actualRecord.ReportingData.Should().Be(_auditRecordDto.ReportingData);
        actualRecord.UpdatedBy.Should().Be(_auditRecordDto.UpdatedBy);
        actualRecord.UpdatedUtc.Should().Be(_auditRecordDto.UpdatedUtc);
    }
}