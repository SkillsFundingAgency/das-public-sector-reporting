using System;
using OpenQA.Selenium;
using SFA.DAS.PSRService.Web.Specflow.Tests.consts;
using SFA.DAS.PSRService.Web.Specflow.Tests.Framework.Helpers;
using SFA.DAS.PSRService.Web.Specflow.Tests.TestSupport;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.Pages
{
    public class ReportSummaryPage : BasePage
    {
        private static String PAGE_TITLE = "Review details";
        
        private readonly By _continueButton = By.Id("report-summary-continue");
        private readonly By _organisationName = By.ClassName("task-list-section");
        private readonly By _errorSummary = By.ClassName("error-summary-heading");

        public ReportSummaryPage(IWebDriver webDriver) : base(webDriver)
        {
        }

        public override void Navigate()
        {
            WebDriver.Url = GetPageUrl(PageUrls.ReportSummary);
        }
        
        public override bool Verify()
        {
            return PageInteractionHelper.VerifyPageHeading(this.GetPageHeading(), PAGE_TITLE);
        }

        public bool DoesContinueButtonExist => PageInteractionHelper.IsElementPresent(_continueButton);

        public bool IsOrganisationNameDisplayed => PageInteractionHelper.IsElementPresent(_organisationName);

        public bool IsErrorSummaryDisplayed => PageInteractionHelper.IsElementPresent(_errorSummary);

        internal void ClickContinue()
        {
            FormCompletionHelper.ClickElement(_continueButton);
        }
    }
}