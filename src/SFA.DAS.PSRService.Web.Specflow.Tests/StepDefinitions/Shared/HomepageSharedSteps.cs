﻿using System;
using SFA.DAS.PSRService.Web.Specflow.Tests.Pages;
using SFA.DAS.PSRService.Web.Specflow.Tests.TestSupport;
using TechTalk.SpecFlow;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.StepDefinitions
{
    [Binding]
    public class HomepageSharedSteps : BaseTest
    {
       
        [When(@"Selects Homepage '(.*)' Radio button")]
        public void WhenSelectsHomepageRadioButton(string p0)
        {
            PsrsHomepage homepage = new PsrsHomepage(webDriver);

            switch (p0)
            {
                case "View a previously submitted report":
                    homepage.SelectPreviouslySubmittedReports();
                    break;
            }
        }
    }
}
