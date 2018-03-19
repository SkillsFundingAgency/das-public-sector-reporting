using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.PSRService.Application.ReportHandlers;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Domain.Enums;
using SFA.DAS.PSRService.Web.Configuration;

namespace SFA.DAS.PSRService.Web.Services
{
    public class ReportService : IReportService
    {
        private readonly IWebConfiguration _config;
        private IMediator _mediator;

        public ReportService(IWebConfiguration config, IMediator mediator)
        {
            _config = config;
            _mediator = mediator;
        }

        public Report CreateReport(long employerId)
        {
            
            var request = new CreateReportRequest(){Period = GetCurrentReportPeriod(), EmployerId = employerId};

            var report = _mediator.Send(request).Result;


            if (report?.Id == null)
            {
                throw new Exception("Unable to create a new report");
            }

            return report;
        }

        public Report GetReport(string period, long employerId)
        {
            var request = new GetReportRequest() {Period = period, EmployerId = employerId};
            var report = _mediator.Send(request).Result;
            return report;
        }

        public SubmittedStatus SubmitReport(string period, long employerId, Submitted submittedDetails)
        {

            var report = GetReport(period, employerId);

            if (IsSubmitValid(report) == false)
                return SubmittedStatus.Invalid;


            throw new NotImplementedException();
        }

       
        public IEnumerable<Report> GetSubmittedReports(long employerId)
        {
            var request = new GetSubmittedRequest(){EmployerId = employerId};

            var submittedReports = _mediator.Send(request).Result;
            return submittedReports;
        }

        public bool IsSubmitValid(Report report)
        {
            if (report?.Submitted == false && IsCurrentPeriod(report?.ReportingPeriod))
                return true;

            return false;
        }

        public string GetCurrentReportPeriod(DateTime utcToday)
        {
            var year = utcToday.Year;
            if (utcToday.Month < 10) year--;
            return string.Concat(year.ToString(CultureInfo.InvariantCulture).Substring(2), (year + 1).ToString(CultureInfo.InvariantCulture).Substring(2));
        }

        public string GetCurrentReportPeriod()
        {
            return GetCurrentReportPeriod(DateTime.UtcNow.Date);
        }

        public string GetCurrentReportPeriodName(string period)
        {
            if (period == null || period.Length != 4)
                throw new ArgumentException("Period string has to be 4 chars", nameof(period));

            var year = int.Parse(period.Substring(0, 2)) + 2000;

            return $"1 April {year} to 31 March {year + 1}";
        }

        private bool IsCurrentPeriod(string reportingPeriod)
        {
            throw new NotImplementedException();
        }

       
    }
}
