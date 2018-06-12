using System;
using AutoMapper;
using MediatR;
using Newtonsoft.Json;
using SFA.DAS.PSRService.Application.Domain;
using SFA.DAS.PSRService.Application.Interfaces;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Application.ReportHandlers
{
    public class UpdateReportHandler : RequestHandler<UpdateReportRequest>
    {
        private readonly IMapper _mapper;
        private readonly IReportRepository _reportRepository;

        public UpdateReportHandler(IMapper mapper, IReportRepository reportRepository)
        {
            _mapper = mapper;
            _reportRepository = reportRepository;
        }

        protected override void HandleCore(UpdateReportRequest request)
        {
            if (string.IsNullOrEmpty(request.UserName))
                throw new Exception("No User Name Supplied");

            var oldVersion = _reportRepository.Get(request.Report.Id);

            if (oldVersion == null)
                throw new Exception("Failed to get old version of report");

            request.Report.UpdatePercentages();

            var reportDto = _mapper.Map<ReportDto>(request.Report);
            reportDto.UpdatedUtc = DateTime.UtcNow;
            reportDto.UpdatedBy = JsonConvert.SerializeObject(new User {Id = request.UserId, Name = request.UserName});
            if (!reportDto.AuditWindowStartUtc.HasValue)
                reportDto.AuditWindowStartUtc = reportDto.UpdatedUtc;

            if (RequiresAuditRecord(oldVersion, reportDto, request.AuditWindowSize))
            {
                var auditRecord = new AuditRecordDto
                {
                    ReportId = reportDto.Id,
                    ReportingData = oldVersion.ReportingData,
                    UpdatedBy = oldVersion.UpdatedBy,
                    UpdatedUtc = oldVersion.UpdatedUtc.Value
                };

                _reportRepository.SaveAuditRecord(auditRecord);

                reportDto.AuditWindowStartUtc = reportDto.UpdatedUtc.Value;
            }

            _reportRepository.Update(reportDto);
        }

        private static bool RequiresAuditRecord(ReportDto oldVersion, ReportDto newVersion, TimeSpan requestAuditWindowSize)
        {
            if (IsPreAudit(oldVersion)) // report could have been saved before we rolled out audit history
                return false;

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
    }
}