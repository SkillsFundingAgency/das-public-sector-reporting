using System.Collections.Generic;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Web.Models;
using SFA.DAS.PSRService.Web.ViewModels;

namespace SFA.DAS.PSRService.IntegrationTests.ReportSubmission
{
    public  class ReportNumbersAnswersBuilderSchools
    {
        private List<QuestionViewModel> validAnswers;
        private string _currentSectionId;

        public ReportNumbersAnswersBuilderSchools()
        {
            validAnswers = new List<QuestionViewModel>
            {
                new QuestionViewModel {Id = "atStart", Answer = "1"},
                new QuestionViewModel {Id = "atEnd", Answer = "1"},
                new QuestionViewModel {Id = "newThisPeriod", Answer = "1"}
            };
        }
        public ReportNumbersAnswersBuilderSchools BuildValidSchoolsEmployeesAnswers()
        {
            _currentSectionId = "SchoolsEmployees";

            return this;
        }

        public ReportNumbersAnswersBuilderSchools BuildValidSchoolsApprenticesAnswers()
        {
            _currentSectionId = "SchoolsApprentices";

            return this;
        }

        public SectionModel ForReportingPeriodSchools(Period reportingPeriod)
        {
            return new SectionModel
            {
                Id = _currentSectionId,
                ReportingPeriod = reportingPeriod.PeriodString,
                Questions = validAnswers
            };
        }
    }
}
