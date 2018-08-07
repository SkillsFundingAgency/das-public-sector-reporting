using System;
using NUnit.Framework;
using SFA.DAS.PSRService.Web.Specflow.Tests.Pages;
using SFA.DAS.PSRService.Web.Specflow.Tests.Repository;
using SFA.DAS.PSRService.Web.Specflow.Tests.Repository.DataVerification;
using SFA.DAS.PSRService.Web.Specflow.Tests.TestSupport;
using TechTalk.SpecFlow;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.StepDefinitions
{
    [Binding]
    public class ReportEditSteps : BaseTest
    {
        private readonly SQLReportRepository _reportRepository;

        public ReportEditSteps(SQLReportRepository reportRepositoryFromContext)
        {
            _reportRepository = reportRepositoryFromContext;
        }

        [Given(@"the your employees question values (.*), (.*) and (.*) have been edited")]
        public void GivenTheYourEmployeesQuestionValuesAndHaveBeenEdited(string atStart, string atEnd, string newThisPeriod)
        {
            var yourEmployees = pageFactory.QuestionYourEmployees;

            yourEmployees.EditAtStartValue(atStart);
            yourEmployees.EditAtEndValue(atEnd);
            yourEmployees.EditAtNewThisPeriodValue(newThisPeriod);
        }

        [Given(@"the your apprentices question values (.*), (.*) and (.*) have been edited")]
        public void GivenTheYourApprenticesQuestionValuesAndHaveBeenEdited(string atStart, string atEnd, string newThisPeriod)
        {
            var yourApprentices = pageFactory.QuestionYourApprentices;

            yourApprentices.EditAtStartValue(atStart);
            yourApprentices.EditAtEndValue(atEnd);
            yourApprentices.EditAtNewThisPeriodValue(newThisPeriod);
        }

        [Given(@"User answers the Your Employees new at start question with (.*)")]
        public void GivenUserAnswersTheYourEmployeesNewAtStartQuestionWith(int numberOfEmployees)
        {
            pageFactory.QuestionYourEmployees.EditAtStartValue(numberOfEmployees.ToString());
        }

        [Given(@"User answers the Your Employees new at end question with (.*)")]
        public void GivenUserAnswersTheYourEmployeesNewAtEndQuestionWith(int numberOfEmployees)
        {
            pageFactory.QuestionYourEmployees.EditAtEndValue(numberOfEmployees.ToString());
        }

        [Given(@"User answers the Your Employees new this period question with (.*)")]
        public void GivenUserAnswersTheYourEmployeesNewThisPeriodQuestionWith(int numberOfEmployees)
        {
            pageFactory.QuestionYourEmployees.EditAtNewThisPeriodValue(numberOfEmployees.ToString());
        }

        [Given(@"User does not answer the Your Employees new this period question")]
        public void GivenUserDoesNotAnswerNewThisPeriodQuestion()
        {
            //Do nothing
        }

        [Given(@"User answers the Your Apprentices new at start question with (.*)")]
        public void GivenUserAnswersTheYourApprenticesNewAtStartQuestionWith(int numberOfApprentices)
        {
            pageFactory.QuestionYourApprentices.EditAtStartValue(numberOfApprentices.ToString());
        }

        [Given(@"User answers the Your Apprentices new at end question with (.*)")]
        public void GivenUserAnswersTheYourApprenticesNewAtEndQuestionWith(int numberOfApprentices)
        {
            pageFactory.QuestionYourApprentices.EditAtEndValue(numberOfApprentices.ToString());
        }

        [Given(@"User answers the Your Apprentices new this period question with (.*)")]
        public void GivenUserAnswersTheYourApprenticesNewThisPeriodQuestionWith(int numberOfApprentices)
        {
            pageFactory.QuestionYourApprentices.EditAtNewThisPeriodValue(numberOfApprentices.ToString());
        }

        [Given(@"User does not answer the Your Apprentices new this period question")]
        public void GivenUserDoesNotAnswerTheYourApprenticesNewThisPeriodQuestion()
        {
            //Do nothing
        }

        [Given(@"User answers full time equivalents question with (.*)")]
        public void GivenUserAnswersFullTimeEquivalentsQuestionWith(int numberOfFullTimeEquivalents)
        {
            pageFactory.QuestionFullTimeEquivalent.EditFullTimeEquivalents(numberOfFullTimeEquivalents.ToString());
        }

        [Given(@"User answers Outline Actions question with ""(.*)""")]
        public void GivenUserAnswersOutlineActionsQuestionWith(string text)
        {
            pageFactory.QuestionOutlineActions.EditText(text);
        }

        [Given(@"User answers Challenges question with ""(.*)""")]
        public void GivenUserAnswersChallengesQuestionWith(string text)
        {
            pageFactory.QuestionChallenges.EditText(text);
        }

        [Given(@"User answers Target Plans question with '(.*)'")]
        public void GivenUserAnswersTargetPlansQuestionWith(string text)
        {
            pageFactory.QuestionTargetPlans.EditText(text);
        }

        [Given(@"User answers Anything Else question with '(.*)'")]
        public void GivenUserAnswersAnythingElseQuestionWith(string text)
        {
            pageFactory.QuestionAnythingElse.EditText(text);
        }

        [When(@"User clicks on '(.*)' question link")]
        public void WhenUserClicksOnQuestionLink(string p0)
        {
            var linkText = p0.Contains("''") ? p0.Replace("''", "'") : p0;

            pageFactory.ReportEdit.ClickQuestionLink(linkText);
        }

        [When(@"User clicks on the save question")]
        public void WhenUserClicksOnTheSaveQuestion()
        {
            pageFactory.QuestionYourEmployees.SaveQuestionAnswers();
        }

        [Then(@"the edit report page is displayed")]
        public void ThenTheEditReportPageIsDisplayed()
        {
            pageFactory.ReportEdit.Verify();
        }

        [Then(@"the Your employees page is displayed")]
        public YourEmployeesEditPage ThenTheYourEmployeesPageIsDisplayed()
        {
            return pageFactory.QuestionYourEmployees;
        }

        [Then(@"organisation name is pre-populated with name from my account")]
        public void ThenOrganisationNameIsPrePopulatedWithNameFromMyAccount()
        {
            pageFactory.ReportOrganisationName.VerifyOrganisationNameHasValue();
        }

        [When(@"User clicks Continue on Your Employees question page")]
        [Given(@"User clicks Continue on Your Employees question page")]
        public void WhenUserClicksContinueOnYourEmployeesQuestionPage()
        {
            pageFactory.QuestionYourEmployees.SaveQuestionAnswers();
        }

        [When(@"User clicks Continue on Your Apprentices question page")]
        [Given(@"User clicks Continue on Your Apprentices question page")]
        public void WhenUserClicksContinueOnYourApprenticesQuestionPage()
        {
            pageFactory.QuestionYourApprentices.SaveQuestionAnswers();
        }

        [When(@"User clicks Continue on Full time equivalent question page")]
        public void WhenUserClicksContinueOnFullTimeEquivalentQuestionPage()
        {
            pageFactory.QuestionFullTimeEquivalent.SaveQuestionAnswers();
        }

        [When(@"User clicks Continue on Outline Actions question page")]
        public void WhenUserClicksContinueOnOutlineActionsQuestionPage()
        {
            pageFactory.QuestionOutlineActions.SaveQuestionAnswers();
        }

        [When(@"User clicks Continue on Challenges question page")]
        public void WhenUserClicksContinueOnChallengesQuestionPage()
        {
            pageFactory.QuestionChallenges.SaveQuestionAnswers();
        }

        [When(@"User clicks Continue on Target Plans question page")]
        public void WhenUserClicksContinueOnTargetPlansQuestionPage()
        {
            pageFactory.QuestionTargetPlans.SaveQuestionAnswers();
        }

        [When(@"User clicks Continue on Anything Else question page")]
        public void WhenUserClicksContinueOnAnythingElseQuestionPage()
        {
            pageFactory.QuestionAnythingElse.SaveQuestionAnswers();
        }

        [Then(@"User is returned to report edit page")]
        public void ThenUserIsReturnedToReportEditPage()
        {
            pageFactory.ReportEdit.Verify();
        }

        [Then(@"completion status for your employees is COMPLETE")]
        public void ThenCompletionStatusForYourEmployeesIsCOMPLETE()
        {
            pageFactory.ReportEdit.VerifyComplete("YourEmployees");
        }

        [Then(@"no completion status is shown for your employees")]
        public void ThenNoCompletionStatusIsShownForYourEmployees()
        {
            pageFactory.ReportEdit.VerifyIncomplete("YourEmployees");
        }

        [Then(@"completion status for your apprentices is COMPLETE")]
        public void ThenCompletionStatusForYourApprenticesIsCOMPLETE()
        {
            pageFactory.ReportEdit.VerifyComplete("YourApprentices");
        }

        [Then(@"no completion status is shown for your apprentices")]
        public void ThenNoCompletionStatusIsShownForYourApprentices()
        {
            pageFactory.ReportEdit.VerifyIncomplete("YourApprentices");
        }

        [Then(@"completion status for full time equivalent is COMPLETE")]
        public void ThenCompletionStatusForFullTimeEquivalentIsCOMPLETE()
        {
            pageFactory.ReportEdit.VerifyComplete("FullTimeEquivalent");
        }

        [Then(@"no completion status is shown for full time equivalent")]
        public void ThenNoCompletionStatusIsShownForFullTimeEquivalent()
        {
            pageFactory.ReportEdit.VerifyIncomplete("FullTimeEquivalent");
        }

        [Then(@"no completion status is shown for outline actions")]
        public void ThenNoCompletionStatusIsShownForOutlineActions()
        {
            pageFactory.ReportEdit.VerifyIncomplete("OutlineActions");
        }

        [Then(@"completion status for outline actions is COMPLETE")]
        public void ThenCompletionStatusForOutlineActionsIsCOMPLETE()
        {
            pageFactory.ReportEdit.VerifyComplete("OutlineActions");
        }
        
        [Then(@"no completion status is shown for challenges")]
        public void ThenNoCompletionStatusIsShownForChallenges()
        {
            pageFactory.ReportEdit.VerifyIncomplete("Challenges");
        }

        [Then(@"completion status for challenges is COMPLETE")]
        public void ThenCompletionStatusForChallengesIsCOMPLETE()
        {
            pageFactory.ReportEdit.VerifyComplete("Challenges");
        }
        
        [Then(@"no completion status is shown for target plans")]
        public void ThenNoCompletionStatusIsShownForTargetPlans()
        {
            pageFactory.ReportEdit.VerifyIncomplete("TargetPlans");
        }

        [Then(@"completion status for target plans is COMPLETE")]
        public void ThenCompletionStatusForTargetPlansIsCOMPLETE()
        {
            pageFactory.ReportEdit.VerifyComplete("TargetPlans");
        }

        [Then(@"no completion status is shown for anything else")]
        public void ThenNoCompletionStatusIsShownForAnythingElse()
        {
            pageFactory.ReportEdit.VerifyIncomplete("AnythingElse");
        }

        [Then(@"completion status for anything else is COMPLETE")]
        public void ThenCompletionStatusForAnythingElseIsCOMPLETE()
        {
            pageFactory.ReportEdit.VerifyComplete("AnythingElse");
        }

        [Then(@"the confirm submission page is displayed")]
        public void ThenTheConfirmSubmissionageIsDisplayed()
        {
            Assert.True(pageFactory.ReportConfirmation.Verify());
        }

        [Then(@"the edit complete page is displayed")]
        public void ThenTheEditCompletePageIsDisplayed()
        {
            Assert.True(pageFactory.ReportEditComplete.Verify());
        }

        [Then(@"The Your Apprentices question values (.*), (.*) and (.*) have been saved")]
        public void ThenTheYourApprenticesQuestionValuesAndHaveBeenSaved(int atStart, int atEnd, int newThisPeriod)
        {
            pageFactory.ReportEdit.ClickQuestionLink("Number of apprentices who work in England");

            var yourApprentices = pageFactory.QuestionYourApprentices;

            yourApprentices.VerifyAtStartValue(atStart.ToString("N0"));
            yourApprentices.VerifyAtEndValue(atEnd.ToString("N0"));
            yourApprentices.VerifyNewThisPeriodValue(newThisPeriod.ToString("N0"));

            VerifyYourApprenticesAtStartHasBeenPersisted(atStart.ToString());
            VerifyYourApprenticesAtEndHasBeenPersisted(atEnd.ToString());
            VerifyYourApprenticesNewThisPeriodHasBeenPersisted(newThisPeriod.ToString());
        }

        private void VerifyYourApprenticesNewThisPeriodHasBeenPersisted(string newThisPeriod)
        {
            ReportVerifier
                .VerifyReport(GetCurrentReport())
                .YourApprentices
                .NewThisPeriodQuestion
                .HasAnswer(newThisPeriod);
        }

        private void VerifyYourApprenticesAtEndHasBeenPersisted(string atEnd)
        {
            ReportVerifier
                .VerifyReport(GetCurrentReport())
                .YourApprentices
                .AtEndQuestion
                .HasAnswer(atEnd);
        }


        private void VerifyYourApprenticesAtStartHasBeenPersisted(string atStart)
        {
            ReportVerifier
                .VerifyReport(GetCurrentReport())
                .YourApprentices
                .AtStartQuestion
                .HasAnswer(atStart);
        }

        private ReportDto GetCurrentReport()
        {
            return
                _reportRepository
                    .GetReportWithId(
                        ScenarioContext
                            .Current
                            .Get<Guid>(
                                ContextKeys.CurrentReportID));
        }

        [Given(@"the full time equivalents question value (.*) has been edited")]
        public void GivenTheFullTimeEquivalentsQuestionValueHasBeenEdited(int atStart)
        {
            pageFactory
                .QuestionFullTimeEquivalent
                .EditAtStartValue(
                    atStart.ToString());
        }

        [Then(@"The full time equivalents question value (.*) has been saved")]
        public void ThenTheFullTimeEquivalentsQuestionValueHasBeenSaved(int atStart)
        {
            pageFactory.ReportEdit.ClickQuestionLink("Number of full-time equivalents who work in England (optional)");

            pageFactory
                .QuestionFullTimeEquivalent
                .VerifyAtStartValue(atStart.ToString("N0"));

            ReportVerifier
                .VerifyReport(GetCurrentReport())
                .FullTimeEquivalents
                .AtStartQuestion
                .HasAnswer(atStart.ToString());
        }

        [Then(@"The outline actions question value ""(.*)"" has been saved")]
        public void ThenTheOutlineActionsQuestionValueHasBeenSaved(string expectedAnswer)
        {
            Given(@"User navigates to the Outline Actions question page");

            pageFactory
                .QuestionOutlineActions
                .VerifyText(expectedAnswer);

            ReportVerifier
                .VerifyReport(GetCurrentReport())
                .OutlineActions
                .HasAnswer(expectedAnswer);
        }

        [Then(@"The Challenges question value ""(.*)"" has been saved")]
        public void ThenTheChallengesQuestionValueHasBeenSaved(string expectedAnswer)
        {
            Given(@"User navigates to the Challenges question page");

            pageFactory
                .QuestionChallenges
                .VerifyText(expectedAnswer);

            ReportVerifier
                .VerifyReport(GetCurrentReport())
                .Challenges
                .HasAnswer(expectedAnswer);
        }
    }
}
