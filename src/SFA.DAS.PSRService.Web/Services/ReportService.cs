using System;
using System.Collections.Generic;
using MediatR;
using SFA.DAS.PSRService.Application.ReportHandlers;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Domain.Enums;
using SFA.DAS.PSRService.Web.Configuration;

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

        public Report CreateReport(string employerId)
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

            return report;
        }

        public Report GetReport(string period, string employerId)
        {
            var request = new GetReportRequest() { Period = period, EmployerId = employerId };
            var report = _mediator.Send(request).Result;
            return report;
        }

        public Report GetReport(Period period, string employerId)
        {
            return
                GetReport(
                    period.GetReportPeriod()
                    , employerId);
        }

        public SubmittedStatus SubmitReport(string period, string employerId, Submitted submittedDetails)
        {

            var report = GetReport(period, employerId);


            if (report.IsSubmitAllowed == false)
                return SubmittedStatus.Invalid;


            report.SubmittedDetails = submittedDetails;
            var request = new SubmitReportRequest() { Report = report };

            var submitReport = _mediator.Send(request);


            return SubmittedStatus.Submitted;
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
    }
}
