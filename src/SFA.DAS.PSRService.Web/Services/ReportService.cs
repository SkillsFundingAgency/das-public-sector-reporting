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

namespace SFA.DAS.PSRService.Web.Services
{
    public class ReportService : IReportService
    {
        
        private IMediator _mediator;
        private IWebConfiguration _config;

        public ReportService(IWebConfiguration config, IMediator mediator)
        {
            _mediator = mediator;
            _config = config;
        }

        public Report CreateReport(string employerId)
        {
            if (IsSubmissionsOpen() == false)
            {
                throw new Exception("Unable to create report after submissions is closed");
            }
            
            var request = new CreateReportRequest(){Period = GetCurrentReportPeriod(), EmployerId = employerId};

            var report = _mediator.Send(request).Result;


            if (report?.Id == null)
            {
                throw new Exception("Unable to create a new report");
            }

            return report;
        }

        public Report GetReport(string period, string employerId)
        {
            var request = new GetReportRequest() {Period = period, EmployerId = employerId};
            var report = _mediator.Send(request).Result;
            return report;
        }

        public SubmittedStatus SubmitReport(string period, string employerId, Submitted submittedDetails)
        {

            var report = GetReport(period, employerId);

           
            if (IsSubmitValid(report) == false || (report.Sections.All(w => w.CompletionStatus != CompletionStatus.Completed)))
                return SubmittedStatus.Invalid;


            report.SubmittedDetails = submittedDetails;
            var request = new SubmitReportRequest(){Report = report};

            var submitReport = _mediator.Send(request);
            

            return SubmittedStatus.Submitted;
        }

       
        public IEnumerable<Report> GetSubmittedReports(string employerId)
        {
            var request = new GetSubmittedRequest(){EmployerId = employerId};

            var submittedReports = _mediator.Send(request).Result;
            return submittedReports;
        }

        public bool IsSubmitValid(Report report)
        {
            if ((report?.Submitted == false && IsCurrentPeriod(report?.ReportingPeriod)) && IsSubmissionsOpen())
                return true;

            return false;
        }

        public Section GetQuestionSection(string SectionId, Report report)
        {
          

            var sectionsList = GetSections(report);

            return sectionsList.Single(w => w.Id == SectionId);

        }

        public void SaveQuestionSection(Section Section, Report report)
        {
            var section = GetQuestionSection(Section.Id, report);

            section = Section;

            SaveReport(report);
        }


        public string GetCurrentReportPeriod(DateTime utcToday)
        {
            var year = utcToday.Year;
            if (utcToday.Month < 4) year--;
            return string.Concat((year -1).ToString(CultureInfo.InvariantCulture).Substring(2), (year).ToString(CultureInfo.InvariantCulture).Substring(2));
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
            return (GetCurrentReportPeriod() == reportingPeriod);
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

        public ReportingPercentages CalculatePercentages(Report report)
        {
            var percentages = new ReportingPercentages();

            var employeeQuestions = GetQuestionSection("YourEmployees", report);
            var apprenticeQuestions = GetQuestionSection("YourApprentices", report);

            var employmentPeriod = decimal.Parse(employeeQuestions.Questions.Single(w => w.Id == "newThisPeriod").Answer);
            var apprenticePeriod = decimal.Parse(apprenticeQuestions.Questions.Single(w => w.Id == "newThisPeriod").Answer);

            var employmentEnd = decimal.Parse(employeeQuestions.Questions.Single(w => w.Id == "atEnd").Answer);
                var apprenticeEnd = decimal.Parse(apprenticeQuestions.Questions.Single(w => w.Id == "atEnd").Answer);

            var employmentStart = decimal.Parse(employeeQuestions.Questions.Single(w => w.Id == "atStart").Answer);

            percentages.EmploymentStarts = (apprenticePeriod / employmentPeriod)*100;
            percentages.TotalHeadCount = (apprenticeEnd / employmentEnd) * 100;
            percentages.NewThisPeriod = (apprenticePeriod / employmentStart) * 100;


            return percentages;
        }
    }
}
