﻿using System;
using OpenQA.Selenium;
using SFA.DAS.PSRService.Web.Specflow.Tests.consts;
using SFA.DAS.PSRService.Web.Specflow.Tests.Framework.Helpers;
using SFA.DAS.PSRService.Web.Specflow.Tests.TestSupport;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.Pages
{
    public class PreviouslySubmittedReportsPage : BasePage
    {
        private static String PAGE_TITLE = "Submitted reports";

        public PreviouslySubmittedReportsPage(IWebDriver webDriver) : base(webDriver)
        {
        }
        public override void Navigate()
        {
            WebDriver.Url = GetPageUrl(PageUrls.ReportPreviouslySubmittedList);
        }
        public override bool Verify()
        {
            return PageInteractionHelper.VerifyPageHeading(this.GetPageHeading(), PAGE_TITLE);
        }


        public bool VerifyMessageDisplayed(string noReportsMessage)
        {
            return true;
        }

        public IWebElement BackButtonLink => WebDriver.WaitForElementToBeVisible(By.ClassName("link-back"));

        public IWebElement NoSubmittedReportDisplayMessage => WebDriver.WaitForElementToBeVisible(By.CssSelector(@"[data-automated-test-id=""submitted-report-list-display-message""]"));

        public IWebElement SubmittedReportDisplayed => WebDriver.WaitForElementToBeVisible(By.CssSelector(@"[data-automated-test-id=""submitted-report-list""]"));

        public int SubmittedReportListCount
        {
            get
            {
                var reportItemRows = WebDriver.FindElements(By.CssSelector(@"[data-automated-test-id=""submitted-report-item""]"));
                return reportItemRows != null ? reportItemRows.Count : 0;
            }
        }
    }
}