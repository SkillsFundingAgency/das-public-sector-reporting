using System;
using System.Linq;
using OpenQA.Selenium;
using SFA.DAS.PSRService.Web.Specflow.Tests.consts;
using SFA.DAS.PSRService.Web.Specflow.Tests.Framework.Helpers;
using SFA.DAS.PSRService.Web.Specflow.Tests.TestSupport;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.Pages
{
    public class ReportEditPage : BasePage
    {
        private static String PAGE_TITLE = "Reporting your progress towards the public sector apprenticeship target";

        public ReportEditPage(IWebDriver webDriver) : base(webDriver)
        {
        }

        public override void Navigate()
        {
            WebDriver.Url = GetPageUrl(PageUrls.ReportEdit);
        }

        public override bool Verify()
        {
            return PageInteractionHelper.VerifyPageHeading(this.GetPageHeading(), PAGE_TITLE);
        }

        public void ClickQuestionLink(string linkText)
        {
            FormCompletionHelper.ClickElement(By.LinkText(linkText));
        }

        public void ClickReportSummaryLink(string linkText)
        {

        }

        internal bool VerifyComplete(string questionId)
        {
            if (IsComplete(questionId))
            {
                return true;
            }

            throw new Exception($"Question id {questionId} was not found or was not complete.");
        }

        internal bool VerifyIncomplete(string questionId)
        {
            if (!IsComplete(questionId))
            {
                return true;
            }

            throw new Exception($"A question with id {questionId} was not found or was already complete.");
        }

        private bool IsComplete(string questionId)
        {
            try
            {
                PageInteractionHelper.TurnOffImplicitWaits();

                var questions = WebDriver.FindElements(By.ClassName("task-list-item"));

                foreach (var question in questions)
                {
                    if (question.FindElements(By.Id(questionId)).Any())
                    {
                        var sv = question.FindElements(By.ClassName("task-completed")).FirstOrDefault();
                        if (sv != null
                            && String.Compare(sv.Text, "COMPLETE", StringComparison.InvariantCultureIgnoreCase) == 0)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
            finally
            {
                PageInteractionHelper.TurnOnImplicitWaits();
            }
        }
    }
}