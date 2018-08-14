using System;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using SFA.DAS.PSRService.Web.Specflow.Tests.Repository;
using SFA.DAS.PSRService.Web.Specflow.Tests.Repository.DataVerification;
using SFA.DAS.PSRService.Web.Specflow.Tests.TestSupport;
using TechTalk.SpecFlow;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.StepDefinitions.Shared
{
    [Binding]
    public class ReportSharedSteps : BaseTest
    {
        private readonly SQLReportRepository _reportRepository;

        private ReportDto _reportDto = new ReportDto();

        public ReportSharedSteps(SQLReportRepository repositoryFromContext)
        {
            _reportRepository = repositoryFromContext;

            var reportId = new Guid("34C46350-6383-4FB8-823D-8DC189862400");

            ScenarioContext.Current.Set(reportId, ContextKeys.CurrentReportID);

            _reportDto.Id = reportId;

            _reportDto.EmployerId = Configurator.GetConfiguratorInstance().GetEmployerId();
            _reportDto.ReportingPeriod = new Period(DateTime.UtcNow).PeriodString;
            _reportDto.ReportingData = "{}";
        }

        [Given(@"a report has been created")]
        public void GivenAReportHasBeenCreated()
        {
            _reportDto.ReportingPeriod = new Period(DateTime.UtcNow).PeriodString;
            _reportDto.ReportingData = ReadNewlyCreatedReportData();

            _reportRepository.Create(_reportDto);
        }
        
        [Given(@"no current report exists")]
        public void GivenNoCurrentReportExists()
        {
            DeleteReports();
        }

        [Given(@"A Current report exists")]
        public void GivenACurrentReportExists()
        {
            _reportDto.ReportingPeriod = new Period(DateTime.UtcNow).PeriodString;
            _reportRepository.Create(_reportDto);
        }

        [Given(@"the report has not been submitted")]
        public void GivenTheReportHasNotBeenSubmitted()
        {
            _reportDto.Submitted = false;
            _reportRepository.Update(_reportDto);
        }

        [Given(@"A valid report")]
        public void GivenAValidReport()
        {
            _reportDto.ReportingData = ReadValidReportData();
            _reportRepository.Update(_reportDto);
        }

        [Given(@"A valid report has been created")]
        public void GivenAValidReportHasBeenCreated()
        {
            _reportDto.ReportingPeriod = new Period(DateTime.UtcNow).PeriodString;
            _reportDto.ReportingData = ReadValidReportData();

            _reportRepository.Create(_reportDto);
        }

        [Given(@"the report has been submitted")]
        public void GivenTheReportHasBeenSubmitted()
        {
            _reportDto.Submitted = true;
            _reportRepository.Update(_reportDto);
        }

        [Given(@"An invalid report")]
        public void GivenAnInvalidReport()
        {
            _reportDto.ReportingData = ReadInvalidReportData();
            _reportRepository.Update(_reportDto);
        }
        
        [Given(@"The current report user has been set")]
        public void GivenTheCurrentReportUserHasBeenSet()
        {
            var userId = "0b7a9411-ca0b-401e-9008-4aa3c1f7e0c1";
            var userName = "Sender 2";
            //TODO: Need to get name and id from config - these need to be set to claims values
            //var userId = Configurator.GetConfiguratorInstance().GetEditUserId();
            //var userName = Configurator.GetConfiguratorInstance().GetEditUserName();
            //var userString = "{ \"Id\":\"0b7a9411-ca0b-401e-9008-4aa3c1f7e0c1\",\"Name\":\"Sender 2\"}";
            var userString = $"{{\"Id\":\"{userId}\"  ,\"Name\":\"{userName}\"}}";
            _reportDto.UpdatedBy = userString;
            _reportRepository.UpdateUser(_reportDto);
        }

        [Given(@"The current report was created '(.*)' minutes in the past")]
        public void GivenTheCurrentReportWasCreatedMinutesInThePast(int minutes)
        {
            _reportDto.UpdatedUtc = DateTime.UtcNow.AddMinutes(-1 * minutes);
            _reportDto.AuditWindowStartUtc = DateTime.UtcNow.AddMinutes(-1 * minutes);
            _reportRepository.UpdateTime(_reportDto);
        }

        [Then(@"a new report is created")]
        public void ThenANewReportIsCreated()
        {
            var currentReportDto = _reportRepository.Get(_reportDto.ReportingPeriod, _reportDto.EmployerId);
            Assert.NotNull(currentReportDto);
        }

        [Then(@"the report should be submitted")]
        public void ThenTheReportShouldBeSubmitted()
        {
            var currentReportDto = _reportRepository.Get(_reportDto.ReportingPeriod, _reportDto.EmployerId);
            Assert.True(currentReportDto.Submitted);
        }

        [Then(@"User is returned to report edit page")]
        public void ThenUserIsReturnedToReportEditPage()
        {
            pageFactory.ReportEdit.Verify();
        }

        [Then(@"the Report Summary page should be displayed")]
        public void ThenTheReportSummaryPageIsDisplayed()
        {
            pageFactory.ReportSummary.Verify();
        }

        [Then(@"the Report Summary page with submitted report details should be displayed")]
        public void ThenTheReportSummaryPageWithSubmittedReportDetailsIsDisplayed()
        {
            pageFactory.SubmittedReportSummary.Verify();
        }

        [Then(@"the submit report confirmation page should be displayed")]
        public void ThenTheSubmitReportConfirmationPageIsDisplayed()
        {
            pageFactory.ReportConfirmation.Verify();
        }

        [Then(@"the report submitted page should be displayed")]
        public void ThenTheReportSubmittedPageIsDisplayed()
        {
            pageFactory.ReportSubmitConfirmation.Verify();
        }

        [BeforeScenario]
        public void DeleteReports()
        {
            _reportRepository.Delete(_reportDto.EmployerId);
        }

        private string ReadInvalidReportData()
        {
            return ReadResourceString(@"SFA.DAS.PSRService.Web.Specflow.Tests.StepDefinitions.Shared.ReportData.InvalidReportData.json");
        }

        private string ReadValidReportData()
        {
            return ReadResourceString(@"SFA.DAS.PSRService.Web.Specflow.Tests.StepDefinitions.Shared.ReportData.ValidReportData.json");
        }

        private string ReadNewlyCreatedReportData()
        {
            return ReadResourceString(@"SFA.DAS.PSRService.Web.Specflow.Tests.StepDefinitions.Shared.ReportData.NewlyCreatedReportData.json");
        }

        private static string ReadResourceString(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            string readData;

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (var reader = new StreamReader(stream))
                {
                    readData = reader.ReadToEnd();
                }
            }

            return readData;
        }

        [Given(@"User creates a new report")]
        public void GivenUserCreatesANewReport()
        {
            pageFactory.Homepage.Navigate();
            pageFactory.Homepage.SelectCreateReport();
            pageFactory.Homepage.ClickContinueButton();
            pageFactory.ReportCreate.ClickStartButton();
        }

        [Then(@"The Your Employees question values (.*), (.*) and (.*) have been saved")]
        public void ThenTheYourEmployeesQuestionValuesHaveBeenSaved(int atStart, int atEnd, int newThisPeriod)
        {
            VerifyYourEmployeesAtStartHasBeenPersisted(atStart.ToString());
            VerifyYourEmployeesAtEndHasBeenPersisted(atEnd.ToString());
            VerifyYourEmployeesNewThisPeriodHasBeenPersisted(newThisPeriod.ToString());
        }

        private void VerifyYourEmployeesNewThisPeriodHasBeenPersisted(string newThisPeriod)
        {
            ReportVerifier
                .VerifyReport(_reportRepository.GetReportWithId(_reportDto.Id))
                .YourEmployees
                .NewThisPeriodQuestion
                .HasAnswer(newThisPeriod);
        }

        private void VerifyYourEmployeesAtEndHasBeenPersisted(string atEnd)
        {
            ReportVerifier
                .VerifyReport(_reportRepository.GetReportWithId(_reportDto.Id))
                .YourEmployees
                .AtEndQuestion
                .HasAnswer(atEnd);
        }

        private void VerifyYourEmployeesAtStartHasBeenPersisted(string atStart)
        {
            ReportVerifier
                .VerifyReport(_reportRepository.GetReportWithId(_reportDto.Id))
                .YourEmployees
                .AtStartQuestion
                .HasAnswer(atStart);
        }
    }
}
