using System.Collections.Generic;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Web.Models;
using SFA.DAS.PSRService.Web.ViewModels;

namespace SFA.DAS.PSRService.IntegrationTests.ReportSubmission
{
    public  class ReportNumbersAnswersBuilder
    {
        private List<QuestionViewModel> validAnswers;
        private string _currentSectionId;

        public ReportNumbersAnswersBuilder()
        {
            validAnswers = new List<QuestionViewModel>
            {
                new QuestionViewModel {Id = "atStart", Answer = "1"},
                new QuestionViewModel {Id = "atEnd", Answer = "1"},
                new QuestionViewModel {Id = "newThisPeriod", Answer = "1"}
            };
        }
        public ReportNumbersAnswersBuilder BuildValidYourEmployeesAnswers()
        {
            _currentSectionId = "YourEmployees";

            return this;
        }

        public ReportNumbersAnswersBuilder BuildValidYourApprenticesAnswers()
        {
            _currentSectionId = "YourApprentices";

            return this;
        }

        public SectionModel ForReportingPeriod(Period reportingPeriod)
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
