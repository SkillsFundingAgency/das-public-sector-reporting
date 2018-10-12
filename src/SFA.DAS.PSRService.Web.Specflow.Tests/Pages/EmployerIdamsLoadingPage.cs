using System;
using OpenQA.Selenium;
using SFA.DAS.PSRService.Web.Specflow.Tests.Framework.Helpers;
using SFA.DAS.PSRService.Web.Specflow.Tests.TestSupport;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.Pages
{
    public class EmployerIdamsLoadingPage : BasePage
    {
        private static String PAGE_TITLE = "Loading...";

        public EmployerIdamsLoadingPage(IWebDriver webDriver) : base(webDriver)
        {
        }
        public override void Navigate()
        {
            throw new NotImplementedException("Page should never be called directly");
        }
        public override bool Verify()
        {
            return PageInteractionHelper.VerifyPageHeading(this.GetPageHeading(), PAGE_TITLE);
        }
    }
}