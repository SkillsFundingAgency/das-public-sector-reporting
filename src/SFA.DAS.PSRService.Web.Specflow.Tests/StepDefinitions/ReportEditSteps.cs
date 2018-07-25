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

        [Then(@"The Your Employees question values (.*), (.*) and (.*) have been saved")]
        public void ThenTheYourEmployeesQuestionValuesAndHaveBeenSaved(string p0, string p1, string p2)
        {
            pageFactory.ReportEdit.ClickQuestionLink("Number of employees who work in England");

            var yourEmployees = pageFactory.QuestionYourEmployees;

            yourEmployees.VerifyAtStartValue(p0);
            yourEmployees.VerifyAtEndValue(p1);
            yourEmployees.VerifyNewThisPeriodValue(p2);

            //TODO: Check that the values have been updated in the repository
        }

        [Then(@"organisation name is pre-populated with name from my account")]
        public void ThenOrganisationNameIsPrePopulatedWithNameFromMyAccount()
        {
            pageFactory.ReportOrganisationName.VerifyOrganisationNameHasValue();
        }
    }
}
