﻿using System;
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
            var yourEmployees = new YourEmployeesPage(webDriver);

            yourEmployees.EditAtStartValue(atStart);
            yourEmployees.EditAtEndValue(atEnd);
            yourEmployees.EditAtNewThisPeriodValue(newThisPeriod);
        }

        [When(@"User clicks on '(.*)' question link")]
        public void WhenUserClicksOnQuestionLink(string p0)
        {
            var reportEdit = new ReportEditPage(webDriver);

            reportEdit.ClickQuestionLink(p0);
        }
        
        [When(@"User clicks on the save question")]
        public void WhenUserClicksOnTheSaveQuestion()
        {
            var yourEmployees = new YourEmployeesPage(webDriver);
            yourEmployees.SaveQuestionAnswers();
        }
        
        [Then(@"the edit report page is displayed")]
        public void ThenTheEditReportPageIsDisplayed()
        {
            var reportEdit = new ReportEditPage(webDriver);

            reportEdit.Verify();
        }
        
        [Then(@"the Your employees page is displayed")]
        public YourEmployeesPage ThenTheYourEmployeesPageIsDisplayed()
        {
            var yourEmployees = new YourEmployeesPage(webDriver);
            return yourEmployees;
        }

        [Then(@"The Your Employees question values (.*), (.*) and (.*) have been saved")]
        public void ThenTheYourEmployeesQuestionValuesAndHaveBeenSaved(string p0, string p1, string p2)
        {
            var reportEdit = new ReportEditPage(webDriver);

            reportEdit.ClickQuestionLink("Number of employees who work in England");

            var yourEmployees = ThenTheYourEmployeesPageIsDisplayed();

            yourEmployees.VerifyAtStartValue(p0);
            yourEmployees.VerifyAtEndValue(p1);
            yourEmployees.VerifyNewThisPeriodValue(p2);

            //TODO: Check that the values have been updated in the repository
        }

    }
}
