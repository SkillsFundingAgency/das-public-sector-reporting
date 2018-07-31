using NUnit.Framework;
using SFA.DAS.PSRService.Web.Specflow.Tests.Pages;
using SFA.DAS.PSRService.Web.Specflow.Tests.TestSupport;
using TechTalk.SpecFlow;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.StepDefinitions
{
    [Binding]
    public class ReportEditSteps : BaseTest
    {
        [Given(@"the question values (.*), (.*) and (.*) have been edited")]
        public void GivenTheQuestionValuesAndHaveBeenEdited(string atStart, string atEnd, string newThisPeriod)
        {
            var yourEmployees = pageFactory.QuestionYourEmployees;

            yourEmployees.EditAtStartValue(atStart);
            yourEmployees.EditAtEndValue(atEnd);
            yourEmployees.EditAtNewThisPeriodValue(newThisPeriod);
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
    }
}
