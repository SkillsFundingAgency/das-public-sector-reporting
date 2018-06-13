using System;
using System.Collections.Generic;
using MediatR;
using SFA.DAS.PSRService.Application.ReportHandlers;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Models;

namespace SFA.DAS.PSRService.Web.Services
{
    public class ReportService : IReportService
    {
        private readonly IMediator _mediator;
        private readonly IWebConfiguration _config;
        private readonly IPeriodService _periodService;

        public ReportService(IWebConfiguration config, IMediator mediator, IPeriodService periodService)
        {
            _mediator = mediator;
            _periodService = periodService;
            _config = config;
        }

        public void CreateReport(string employerId, UserModel user)
        {
            if (IsSubmissionsOpen() == false)
            {
                throw new Exception("Unable to create report after submissions is closed");
            }

            var currentPeriod = _periodService.GetCurrentPeriod();

            var requestUser = new User
            {
                Name = user.DisplayName,
                Id = user.Id
            };

            var request = new CreateReportRequest(requestUser)
            {
                Period = currentPeriod.PeriodString, 
                EmployerId = employerId,
            };

            var report = _mediator.Send(request).Result;


            if (report?.Id == null)
            {
                throw new Exception("Unable to create a new report");
            }
        }

        public Report GetReport(string period, string employerId)
        {
            var request = new GetReportRequest { Period = period, EmployerId = employerId };
            var report = _mediator.Send(request).Result;
            return report;
        }

        public void SubmitReport(Report report)
        {            
            if (!CanBeEdited(report) || !report.IsValidForSubmission())
                throw new InvalidOperationException("Report is invalid for submission.");

            _mediator.Send(new SubmitReportRequest(report));
        }


        public IEnumerable<Report> GetSubmittedReports(string employerId)
        {
            var request = new GetSubmittedRequest() { EmployerId = employerId };

            var submittedReports = _mediator.Send(request).Result;
            return submittedReports;
        }
        

        public bool IsSubmissionsOpen()
        {
            return DateTime.UtcNow < _config.SubmissionClose;
        }

        public void SaveReport(Report report, UserModel user)
        {
            var reqestUser = new User
            {
                Name = user.DisplayName,
                Id = user.Id
            };

            var request = new UpdateReportRequest(report, reqestUser);

            if (_config.AuditWindowSize.HasValue)
                request.AuditWindowSize = _config.AuditWindowSize.Value;

            _mediator.Send(request);
        }

        public bool CanBeEdited(Report report)
        {
            return report != null 
                   && !report.Submitted 
                   && report.Period.IsCurrent 
                   && _periodService.IsSubmissionsOpen();
        }

        public IEnumerable<AuditRecord> GetReportEditHistoryMostRecentFirst(
            Period period, 
            string employerId)
        {
            var request = new GetReportEditHistoryMostRecentFirst(period, employerId);

            return
                _mediator.Send(request).Result;
        }
    }
}
