using System;
using OpenQA.Selenium;
using SFA.DAS.PSRService.Web.Specflow.Tests.Framework.Helpers;
using SFA.DAS.PSRService.Web.Specflow.Tests.TestSupport;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.Pages
{
    public class PreviouslySubmittedReportsPage : BasePage
    {
        private static String PAGE_TITLE = "Submitted reports";

        public PreviouslySubmittedReportsPage(IWebDriver webDriver) : base(webDriver)
        {
            SelfVerify();
        }

        protected override bool SelfVerify()
        {
            return PageInteractionHelper.VerifyPageHeading(this.GetPageHeading(), PAGE_TITLE);
        }


        public bool VerifyMessageDisplayed(string noReportsMessage)
        {
            return true;
        }
    }
}