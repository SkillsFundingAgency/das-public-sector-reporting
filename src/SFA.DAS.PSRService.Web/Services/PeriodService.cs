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
    public class PeriodService : IPeriodService
    {

        private IWebConfiguration _config;

        public PeriodService(IWebConfiguration config)
        {
            _config = config;
        }


        public Period GetCurrentPeriod()
        {
            return new Period(DateTime.UtcNow);
        }

        public bool IsSubmissionsOpen()
        {
            return DateTime.UtcNow < _config.SubmissionClose;
        }

        public bool ReportIsForCurrentPeriod(Report report)
        {
            throw new NotImplementedException();
        }
    }
}
