using System;
using AutoMapper;
using MediatR;
using Newtonsoft.Json;
using SFA.DAS.NServiceBus;
using SFA.DAS.PSRService.Application.Domain;
using SFA.DAS.PSRService.Application.Interfaces;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Messages.Events;

namespace SFA.DAS.PSRService.Application.ReportHandlers
{
    public class UpdateReportHandler : RequestHandler<UpdateReportRequest>
    {
        private readonly IMapper _mapper;
        private readonly IReportRepository _reportRepository;
        private readonly IEventPublisher _eventPublisher;

        public UpdateReportHandler(
            IMapper mapper, 
            IReportRepository reportRepository, 
            IEventPublisher eventPublisher)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _reportRepository = reportRepository ?? throw new ArgumentNullException(nameof(reportRepository));
            _eventPublisher = eventPublisher ?? throw new ArgumentNullException(nameof(eventPublisher));
        }

        protected override void HandleCore(UpdateReportRequest request)
        {
            var oldVersion = _reportRepository.Get(request.Report.Id);

            if (oldVersion == null)
                throw new Exception("Failed to get old version of report");

            request.Report.UpdatePercentages();

            var updatedEvent = _mapper.Map<ReportUpdated>(request.Report);
            
            if(updatedEvent == null)
                throw new InvalidOperationException("Cannot create update event for request report.");
            
            var reportDto = _mapper.Map<ReportDto>(request.Report);
            reportDto.UpdatedUtc = DateTime.UtcNow;
            reportDto.UpdatedBy = JsonConvert.SerializeObject(new User {Id = request.User.Id, Name = request.User.Name});
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

            _eventPublisher.Publish(updatedEvent);
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