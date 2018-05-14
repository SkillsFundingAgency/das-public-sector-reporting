using System.Collections.Generic;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Web.ViewModels;

namespace SFA.DAS.PSRService.IntegrationTests.ReportSubmission
{
    public class FactorsAnswersBuilder
    {
        private List<QuestionViewModel> validAnswers = new List<QuestionViewModel>(1);
        private string currentSectionId;

        public FactorsAnswersBuilder BuildValidOutlineActionsAnswer()
        {
            currentSectionId = "OutlineActions";

            ClearAnswersAndAddNewAnswerForId(
                currentSectionId);

            return this;
        }

        public FactorsAnswersBuilder BuildValidChallengesAnswer()
        {
            currentSectionId = "Challenges";

            ClearAnswersAndAddNewAnswerForId(
                currentSectionId);

            return this;
        }

        public FactorsAnswersBuilder BuildValidTargetPlansAnswer()
        {
            currentSectionId = "TargetPlans";

            ClearAnswersAndAddNewAnswerForId(
                currentSectionId);

            return this;
        }

        public SectionViewModel ForReportingPeriod(Period reportingPeriod)
        {
            return new SectionViewModel
            {
                Report = new Report {Period = reportingPeriod},
                CurrentSection = new Section
                {
                    Id = currentSectionId
                },
                Questions = validAnswers
            };
        }

        private void ClearAnswersAndAddNewAnswerForId(string answerId)
        {
            validAnswers.Clear();

            validAnswers
                .Add(
                    new QuestionViewModel
                        {Id = answerId, Answer = $"Some valid text for {answerId}"});
        }
    }
}
