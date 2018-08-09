using System;
using System.Diagnostics.CodeAnalysis;
using SFA.DAS.PSRService.Web.Specflow.Tests.Repository;
using SFA.DAS.PSRService.Web.Specflow.Tests.Repository.DataVerification;
using SFA.DAS.PSRService.Web.Specflow.Tests.TestSupport;
using TechTalk.SpecFlow;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.StepDefinitions
{
    [ExcludeFromCodeCoverage]
    [Binding]
    public class QuestionOrganisationNameSteps : BaseTest
    {
        private readonly SQLReportRepository _reportRepository;

        public QuestionOrganisationNameSteps(SQLReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        [Given(@"User answers Organisation Name question with ""(.*)""")]
        public void GivenUserAnswersOrganisationNameQuestionWith(string text)
        {
            pageFactory.ReportOrganisationName.EditOrganisationName(text);
        }

        [When(@"User clicks Continue on Organisation Name question page")]
        public void WhenUserClicksContinueOnOrganisationNameQuestionPage()
        {
            pageFactory.ReportOrganisationName.ClickContinue();
        }

        [Then(@"The organisation question value ""(.*)"" has been saved")]
        public void ThenTheOrganisationQuestionValueHasBeenSaved(string expectedAnswer)
        {
            ReportVerifier
                .VerifyReport(GetCurrentReport())
                .OrganisationName
                .HasAnswer(expectedAnswer);
        }

        private ReportDto GetCurrentReport()
        {
            return
                _reportRepository
                    .GetReportWithId(
                        ScenarioContext
                            .Current
                            .Get<Guid>(
                                ContextKeys.CurrentReportID));
        }
    }
}