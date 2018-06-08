using System;
using OpenQA.Selenium;
using SFA.DAS.PSRService.Web.Specflow.Tests.Framework.Helpers;
using SFA.DAS.PSRService.Web.Specflow.Tests.TestSupport;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.Pages
{
    public abstract class QuestionEditPage : BasePage
    {
        protected QuestionEditPage(IWebDriver webDriver) : base(webDriver)
        {
           
        }

        public void EditAnswer(By answerLocation, string value)
        {
            FormCompletionHelper.EnterText(answerLocation, value);
        }
        
        private readonly By _submitButton = By.CssSelector("button[type='submit']");
        public void SaveQuestionAnswers()
        {
            FormCompletionHelper.ClickElement(_submitButton);
        }
    }
}