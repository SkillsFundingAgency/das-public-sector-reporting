using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using SFA.DAS.PSRService.Web.Specflow.Tests.Framework.Helpers;
using SFA.DAS.PSRService.Web.Specflow.Tests.TestSupport;
using TechTalk.SpecFlow;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.StepDefinitions
{
    public class QuestionValidationFormNumbersSteps : BaseTest
    {
        private const string NoJavascriptTag = "nojavascript";

        [Given(@"javascript is turned off")]
        public void GivenJavascriptIsTurnedOff()
        {
            ScenarioContext.Current.Add(NoJavascriptTag, true);
        }

        [Given(@"user navigates to the Your Employee question page")]
        public void GivenUserNavigatesToTheYourEmployeeQuestionPage()
        {
            pageFactory.QuestionYourEmployees.Navigate();
        }


        [When(@"user enters any value in this page other than a whole number in one or more fields")]
        public void WhenUserEntersAnyValueInThisPageOtherThanAWholeNumberInOneOrMoreFields()
        {
            var page = pageFactory.QuestionYourEmployees;

            TurnOffJavaScriptBehaviour();

            page.EditAtStartValue("1x");
            page.EditAtEndValue("2x");
            page.EditAtNewThisPeriodValue("3");
        }

        [When(@"clicks the '(.*)' button")]
        public void WhenClicksTheButton(string p0)
        {
            TurnOffJavaScriptBehaviour();

            pageFactory.QuestionYourEmployees.SaveQuestionAnswers();
        }

        [Then(@"display the following message '(.*)'")]
        public void ThenDisplayTheFollowingMessage(string p0)
        {
            var errorsPresent = PageInteractionHelper.IsElementPresent(By.ClassName("error-summary-heading"));
            var errorsDisplayed = PageInteractionHelper.IsElementDisplayed(By.ClassName("error-summary-heading"));

            if (!errorsPresent || !errorsDisplayed)
            {
                throw new Exception("Errors should be displayed");   
            }
        }

        [Then(@"the '(.*)' question page is displayed")]
        public void ThenTheQuestionPageIsDisplayed(string p0)
        {
            pageFactory.QuestionYourEmployees.Verify();
        }

        private void TurnOffJavaScriptBehaviour()
        {
            var js = "elems = $('.js-addcommas'); " +
                     "elems.off('input');";

            webDriver.ExecuteJavaScript(js);
        }
    }
}
