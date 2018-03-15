﻿using System;
using System.Collections.Generic;
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
            throw new NotImplementedException();
        }

        public Report GetReport(string period, long employeeId)
        {
            var request = new GetReportRequest() {Period = period, EmployeeId = employeeId};
            var report = _mediator.Send(request).Result;
            return report;
        }

        public SubmittedStatus SubmitReport(string period, long employeeId, Submitted submittedDetails)
        {

            var report = GetReport(period, employeeId);

            if (IsSubmitValid(report) == false)
                return SubmittedStatus.Invalid;


            throw new NotImplementedException();
        }

       
        public IList<Report> GetReports(long employerId)
        {
            throw new NotImplementedException();
        }

        public bool IsSubmitValid(Report report)
        {
            throw new NotImplementedException();
        }

        private bool IsCurrentPeriod(string reportingPeriod)
        {
            throw new NotImplementedException();
        }
        public bool IsEditValid(Report report)
        {
            if (report?.Submitted == false && IsCurrentPeriod(report?.ReportingPeriod))
                return true;

            return false;
        }

       
    }
}
