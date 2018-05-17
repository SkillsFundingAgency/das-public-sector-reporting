using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using MediatR;
using SFA.DAS.PSRService.Application.ReportHandlers;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Domain.Enums;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.ViewModels;
using SFA.DAS.PSRService.Web.Models;

namespace SFA.DAS.PSRService.Web.Services
{
    public class ReportService : IReportService
    {
        private IMediator _mediator;
        private IWebConfiguration _config;
        private IPeriodService _periodService;

        public ReportService(IWebConfiguration config, IMediator mediator, IPeriodService periodService)
        {
            _mediator = mediator;
            _periodService = periodService;
            _config = config;
        }

        public void CreateReport(string employerId)
        {
            if (IsSubmissionsOpen() == false)
            {
                throw new Exception("Unable to create report after submissions is closed");
            }

            var currentPeriod = _periodService.GetCurrentPeriod();

            var request = new CreateReportRequest() { Period = currentPeriod.PeriodString, EmployerId = employerId };

            var report = _mediator.Send(request).Result;


            if (report?.Id == null)
            {
                throw new Exception("Unable to create a new report");
            }
        }

        public Report GetReport(string period, string employerId)
        {
            var request = new GetReportRequest() { Period = period, EmployerId = employerId };
            var report = _mediator.Send(request).Result;
            return report;
        }

        public void SubmitReport(Report report)
        {            
            if (!CanBeEdited(report) || !report.IsValidForSubmission())
                throw new Exception("Report is invalid for submission.");

            _mediator.Send(new SubmitReportRequest { Report = report });
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

        public void SaveReport(Report report)
        {
            var request = new UpdateReportRequest { Report = report };
            _mediator.Send(request);
        }

        public bool CanBeEdited(Report report)
        {
            return report != null 
                   && !report.Submitted 
                   && report.Period.IsCurrent 
                   && _periodService.IsSubmissionsOpen();
        }
    }
}
