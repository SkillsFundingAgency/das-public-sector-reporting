﻿using System;
using OpenQA.Selenium;
using SFA.DAS.PSRService.Web.Specflow.Tests.consts;
using SFA.DAS.PSRService.Web.Specflow.Tests.Framework.Helpers;
using SFA.DAS.PSRService.Web.Specflow.Tests.TestSupport;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.Pages
{
    public class ReportOrganisationNamePage : BasePage
    {
        private static String PAGE_TITLE = "Your organisation";
        
        private readonly By _continueButton = By.CssSelector("button[type='submit']");
        private readonly By _organisationName = By.Id("Report_OrganisationName");

        public ReportOrganisationNamePage(IWebDriver webDriver) : base(webDriver)
        {
        }

        public override void Navigate()
        {
            WebDriver.Url = GetPageUrl(PageUrls.ReportOrganisationName);
        }
        
        public override bool Verify()
        {
            return PageInteractionHelper.VerifyPageHeading(this.GetPageHeading(), PAGE_TITLE);
        }

        public bool VerifyOrganisationNameHasValue()
        {
            return PageInteractionHelper.VerifyValueIsNotNullOrEmpty(_organisationName);
        }

        internal void ClickContinue()
        {
            FormCompletionHelper.ClickElement(_continueButton);
        }
    }
}