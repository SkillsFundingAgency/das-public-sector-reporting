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

        private readonly By _backButtonLink = By.ClassName("link-back");
        private readonly By _continueButton = By.Id("report-summary-continue");
        private readonly By _organisationName = By.ClassName("task-list-section");
        private readonly By _errorSummary = By.ClassName("error-summary-heading");
        private readonly By _reportNotYetCreated = By.Id("report-summary-no-reports-message");

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

        public bool IsNoReportCreatedDisplayed => PageInteractionHelper.IsElementPresent(_reportNotYetCreated);
        
        internal void ClickBackButtonLink()
        {
            FormCompletionHelper.ClickElement(_backButtonLink);
        }

        internal void ClickContinue()
        {
            FormCompletionHelper.ClickElement(_continueButton);
        }

        public bool ReportingPercentagesEmploymentStartsIs(decimal expectedPercentage)
        {
            return
                PageInteractionHelper
                    .VerifyText(
                        By.Id("reportingpercentages-employmentstarts"),
                        expectedPercentage.ToString("0.00")+"%");
        }

        public bool ReportingPercentagesTotalHeadCountIs(decimal expectedPercentage)
        {
            return
                PageInteractionHelper
                    .VerifyText(
                        By.Id("reportingpercentages-totalheadcount"),
                        expectedPercentage.ToString("0.00")+"%");
        }

        public bool ReportingPercentagesNewThisPeriodIs(decimal expectedPercentage)
        {
            return
                PageInteractionHelper
                    .VerifyText(
                        By.Id("reportingpercentages-newthisperiod"),
                        expectedPercentage.ToString("0.00")+"%");
        }
    }
}