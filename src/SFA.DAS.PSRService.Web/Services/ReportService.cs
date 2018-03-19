using System;
using System.Collections.Generic;
using System.Linq;
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
        
        private IMediator _mediator;

        public ReportService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Report CreateReport(long employerId)
        {
            throw new NotImplementedException();
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

        public Section GetQuestionSection(string SectionId, Report report)
        {
          

            var sectionsList = GetSections(report);

            return sectionsList.Single(w => w.Id == SectionId);

        }

        private bool IsCurrentPeriod(string reportingPeriod)

        {
            throw new NotImplementedException();
        }

        private IList<Section> GetSections(Report report)
        {
            List<Section> sectionList = new List<Section>();

            if (report.Sections == null)
                throw new Exception("No sections found for report, there must at least one section");

            foreach (var reportSection in report.Sections)
            {
                sectionList.Add(reportSection);
                sectionList.AddRange(GetSections(reportSection));
            }

            return sectionList;
        }

        private IEnumerable<Section> GetSections(Section section)
        {
            List<Section> sectionList = new List<Section>();
            sectionList.Add(section);

            if (section.SubSections != null)
            {
                foreach (var reportSection in section.SubSections)
                {

                    sectionList.AddRange(GetSections(reportSection));
                }
            }
          

            return sectionList;
        }
    }
}
