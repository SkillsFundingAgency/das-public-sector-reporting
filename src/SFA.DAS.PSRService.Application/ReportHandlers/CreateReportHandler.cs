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

public class CreateReportHandler(IReportRepository reportRepository, IMapper mapper, IFileProvider fileProvider)
    : IRequestHandler<CreateReportRequest, Report>
{
    public async Task<Report> Handle(CreateReportRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Period))
        {
            throw new Exception("Period must be supplied");
        }

        var reportDto = new ReportDto
        {
            EmployerId = request.EmployerId,
            Submitted = false,
            Id = Guid.NewGuid(),
            ReportingPeriod = request.Period,
            ReportingData = await GetQuestionConfig(request.IsLocalAuthority),
            AuditWindowStartUtc = DateTime.UtcNow,
            UpdatedUtc = DateTime.UtcNow,
            UpdatedBy = JsonConvert.SerializeObject(new User { Id = request.User.Id, Name = request.User.Name })
        };

        await reportRepository.Create(reportDto);

        return mapper.Map<Report>(reportDto);
    }

    private async Task<string> GetQuestionConfig(bool isLocalAuthority)
    {
        var questionsConfig = fileProvider.GetFileInfo(isLocalAuthority ? "/LocalAuthorityQuestionConfig.json" : "/QuestionConfig.json");

        await using var jsonContents = questionsConfig.CreateReadStream();
        using var streamReader = new StreamReader(jsonContents);
        return await streamReader.ReadToEndAsync();
    }
}