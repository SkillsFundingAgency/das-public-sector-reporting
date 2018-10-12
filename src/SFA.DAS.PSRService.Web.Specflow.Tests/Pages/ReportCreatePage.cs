using System;
using OpenQA.Selenium;
using SFA.DAS.PSRService.Web.Specflow.Tests.consts;
using SFA.DAS.PSRService.Web.Specflow.Tests.Framework.Helpers;
using SFA.DAS.PSRService.Web.Specflow.Tests.TestSupport;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.Pages
{
    public class ReportCreatePage : BasePage
    {
        private static String PAGE_TITLE = "Report 2017 to 2018 public sector apprenticeship target data";

        private readonly By _startButton = By.Id("report-create-start");

        public ReportCreatePage(IWebDriver webDriver) : base(webDriver)
        {
        }

        public override void Navigate()
        {
            WebDriver.Url = GetPageUrl(PageUrls.ReportCreate);
        }

        public override bool Verify()
        {
            return PageInteractionHelper.VerifyPageHeading(this.GetPageHeading(), PAGE_TITLE);
        }

        public void ClickStartButton()
        {
            FormCompletionHelper.ClickElement(_startButton);
        }
    }
}