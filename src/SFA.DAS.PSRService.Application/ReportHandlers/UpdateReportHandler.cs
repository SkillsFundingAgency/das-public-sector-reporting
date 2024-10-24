using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using SFA.DAS.PSRService.Application.Domain;
using SFA.DAS.PSRService.Application.Interfaces;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Application.ReportHandlers;

public class UpdateReportHandler(IMapper mapper, IReportRepository reportRepository, IFileProvider fileProvider)
    : IRequestHandler<UpdateReportRequest>
{
    public async Task Handle(UpdateReportRequest request, CancellationToken cancellationToken)
    {
        var oldVersion = await reportRepository.Get(request.Report.Id);

        if (oldVersion == null)
        {
            throw new Exception("Failed to get old version of report");
        }

        request.Report.UpdatePercentages();

        var reportDto = mapper.Map<ReportDto>(request.Report);

        if (request.IsLocalAuthority.HasValue)
        {
            if (request.IsLocalAuthority != request.Report.IsLocalAuthority)
                reportDto.ReportingData = await GetQuestionConfig(request.IsLocalAuthority.Value);
        }

        reportDto.UpdatedUtc = DateTime.UtcNow;
        reportDto.UpdatedBy = JsonConvert.SerializeObject(new User { Id = request.User.Id, Name = request.User.Name });
        
        reportDto.AuditWindowStartUtc ??= reportDto.UpdatedUtc;

        if (RequiresAuditRecord(oldVersion, reportDto, request.AuditWindowSize))
        {
            var auditRecord = new AuditRecordDto
            {
                ReportId = reportDto.Id,
                ReportingData = oldVersion.ReportingData,
                UpdatedBy = oldVersion.UpdatedBy,
                UpdatedUtc = oldVersion.UpdatedUtc.Value
            };

            await reportRepository.SaveAuditRecord(auditRecord);

            reportDto.AuditWindowStartUtc = reportDto.UpdatedUtc.Value;
        }

        await reportRepository.Update(reportDto);
    }

    private static bool RequiresAuditRecord(ReportDto oldVersion, ReportDto newVersion, TimeSpan requestAuditWindowSize)
    {
        if (IsPreAudit(oldVersion)) // report could have been saved before we rolled out audit history
        {
            return false;
        }

        var oldUser = JsonConvert.DeserializeObject<User>(oldVersion.UpdatedBy);
        var newUser = JsonConvert.DeserializeObject<User>(newVersion.UpdatedBy);
        var timeSinceLastUpdate = DateTime.UtcNow.Subtract(oldVersion.AuditWindowStartUtc.Value);

        return timeSinceLastUpdate > requestAuditWindowSize // updated more than X minutes ago
               || oldUser.Id != newUser.Id;                  // or by another uer
    }

    private static bool IsPreAudit(ReportDto oldVersion)
    {
        return !oldVersion.AuditWindowStartUtc.HasValue || !oldVersion.UpdatedUtc.HasValue || oldVersion.UpdatedBy == null;
    }

    private async Task<string> GetQuestionConfig(bool isLocalAuthority)
    {
        var questionsConfig = fileProvider.GetFileInfo(isLocalAuthority ? "/LocalAuthorityQuestionConfig.json" : "/QuestionConfig.json");

        await using var jsonContents = questionsConfig.CreateReadStream();
        using var streamReader = new StreamReader(jsonContents);
        return await streamReader.ReadToEndAsync();
    }
}