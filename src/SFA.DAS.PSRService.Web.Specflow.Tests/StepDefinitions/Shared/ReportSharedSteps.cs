using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using SFA.DAS.PSRService.Web.Specflow.Tests.consts;
using SFA.DAS.PSRService.Web.Specflow.Tests.Repository;
using SFA.DAS.PSRService.Web.Specflow.Tests.TestSupport;
using TechTalk.SpecFlow;


namespace SFA.DAS.PSRService.Web.Specflow.Tests.StepDefinitions
{
    [Binding]
    public class ReportSharedSteps : BaseTest
    {

        private SQLReportRepository _reportRepository;

        private ReportDto _reportDto = new ReportDto();

        private string _validReportJson = "{\"OrganisationName\":\"THE INSTITUTION OF OCCUPATIONAL SAFETY AND HEALTH\",\"Questions\":[{\"SubSections\":[{\"SubSections\":null,\"Questions\":[{\"Id\":\"atStart\",\"Answer\":\"321\",\"Optional\":false,\"Type\":0},{\"Id\":\"atEnd\",\"Answer\":\"36\",\"Optional\":false,\"Type\":0},{\"Id\":\"newThisPeriod\",\"Answer\":\"32\",\"Optional\":false,\"Type\":0}],\"Id\":\"YourEmployees\",\"Title\":\"Your employees\",\"SummaryText\":\"Number of employees who work in England\"},{\"SubSections\":null,\"Questions\":[{\"Id\":\"atStart\",\"Answer\":\"142\",\"Optional\":false,\"Type\":0},{\"Id\":\"atEnd\",\"Answer\":\"152\",\"Optional\":false,\"Type\":0},{\"Id\":\"newThisPeriod\",\"Answer\":\"32\",\"Optional\":false,\"Type\":0}],\"Id\":\"YourApprentices\",\"Title\":\"Your apprentices\",\"SummaryText\":\"Number of apprentices who work in England\"},{\"SubSections\":null,\"Questions\":[{\"Id\":\"atStart\",\"Answer\":\"\",\"Optional\":true,\"Type\":0}],\"Id\":\"FullTimeEquivalent\",\"Title\":\"Full time equivalents\",\"SummaryText\":\"Number of full-time equivalents who work in England (optional)\"}],\"Questions\":null,\"Id\":\"ReportNumbers\",\"Title\":\"Report numbers in the following categories\",\"SummaryText\":null},{\"SubSections\":[{\"SubSections\":null,\"Questions\":[{\"Id\":\"OutlineActions\",\"Answer\":\"dsadsada\",\"Optional\":false,\"Type\":2}],\"Id\":\"OutlineActions\",\"Title\":\"Outline any actions you have taken to help you progress towards meeting the public sector target\",\"SummaryText\":\"Outline any actions you have taken to help you progress towards meeting the public sector target\"},{\"SubSections\":null,\"Questions\":[{\"Id\":\"Challenges\",\"Answer\":\"dsadsad\",\"Optional\":false,\"Type\":2}],\"Id\":\"Challenges\",\"Title\":\"Tell us about any challenges you have faced in your efforts to meet the target\",\"SummaryText\":\"Tell us about any challenges you have faced in your efforts to meet the target\"},{\"SubSections\":null,\"Questions\":[{\"Id\":\"TargetPlans\",\"Answer\":\"dsadsadas\",\"Optional\":false,\"Type\":2}],\"Id\":\"TargetPlans\",\"Title\":\"How are you planning to ensure you meet the target in future?\",\"SummaryText\":\"How are you planning to ensure you meet the target in future?\"},{\"SubSections\":null,\"Questions\":[{\"Id\":\"AnythingElse\",\"Answer\":\"\",\"Optional\":true,\"Type\":2}],\"Id\":\"AnythingElse\",\"Title\":\"Do you have anything else you want to tell us? (optional)\",\"SummaryText\":\"Do you have anything else you want to tell us? (optional)\"}],\"Questions\":null,\"Id\":\"Factors\",\"Title\":\"Factors that impacted your ability to meet the target\",\"SummaryText\":null}],\"Submitted\":null,\"ReportingPercentages\":{\"EmploymentStarts\":100.0,\"TotalHeadCount\":422.22222222222222222222222222,\"NewThisPeriod\":9.968847352024922118380062310}}";
        private string _invalidReportJson = "{\"OrganisationName\":\"THE INSTITUTION OF OCCUPATIONAL SAFETY AND HEALTH\",\"Questions\":[{\"SubSections\":[{\"SubSections\":null,\"Questions\":[{\"Id\":\"atStart\",\"Answer\":\"321\",\"Optional\":false,\"Type\":0},{\"Id\":\"atEnd\",\"Answer\":\"36\",\"Optional\":false,\"Type\":0},{\"Id\":\"newThisPeriod\",\"Answer\":\"32\",\"Optional\":false,\"Type\":0}],\"Id\":\"YourEmployees\",\"Title\":\"Your employees\",\"SummaryText\":\"Number of employees who work in England\"},{\"SubSections\":null,\"Questions\":[{\"Id\":\"atStart\",\"Answer\":\"142\",\"Optional\":false,\"Type\":0},{\"Id\":\"atEnd\",\"Answer\":\"\",\"Optional\":false,\"Type\":0},{\"Id\":\"newThisPeriod\",\"Answer\":\"\",\"Optional\":false,\"Type\":0}],\"Id\":\"YourApprentices\",\"Title\":\"Your apprentices\",\"SummaryText\":\"Number of apprentices who work in England\"},{\"SubSections\":null,\"Questions\":[{\"Id\":\"atStart\",\"Answer\":\"\",\"Optional\":true,\"Type\":0}],\"Id\":\"FullTimeEquivalent\",\"Title\":\"Full time equivalents\",\"SummaryText\":\"Number of full-time equivalents who work in England (optional)\"}],\"Questions\":null,\"Id\":\"ReportNumbers\",\"Title\":\"Report numbers in the following categories\",\"SummaryText\":null},{\"SubSections\":[{\"SubSections\":null,\"Questions\":[{\"Id\":\"OutlineActions\",\"Answer\":\"dsadsada\",\"Optional\":false,\"Type\":2}],\"Id\":\"OutlineActions\",\"Title\":\"Outline any actions you have taken to help you progress towards meeting the public sector target\",\"SummaryText\":\"Outline any actions you have taken to help you progress towards meeting the public sector target\"},{\"SubSections\":null,\"Questions\":[{\"Id\":\"Challenges\",\"Answer\":\"dsadsad\",\"Optional\":false,\"Type\":2}],\"Id\":\"Challenges\",\"Title\":\"Tell us about any challenges you have faced in your efforts to meet the target\",\"SummaryText\":\"Tell us about any challenges you have faced in your efforts to meet the target\"},{\"SubSections\":null,\"Questions\":[{\"Id\":\"TargetPlans\",\"Answer\":\"dsadsadas\",\"Optional\":false,\"Type\":2}],\"Id\":\"TargetPlans\",\"Title\":\"How are you planning to ensure you meet the target in future?\",\"SummaryText\":\"How are you planning to ensure you meet the target in future?\"},{\"SubSections\":null,\"Questions\":[{\"Id\":\"AnythingElse\",\"Answer\":\"\",\"Optional\":true,\"Type\":2}],\"Id\":\"AnythingElse\",\"Title\":\"Do you have anything else you want to tell us? (optional)\",\"SummaryText\":\"Do you have anything else you want to tell us? (optional)\"}],\"Questions\":null,\"Id\":\"Factors\",\"Title\":\"Factors that impacted your ability to meet the target\",\"SummaryText\":null}],\"Submitted\":null,\"ReportingPercentages\":{\"EmploymentStarts\":100.0,\"TotalHeadCount\":422.22222222222222222222222222,\"NewThisPeriod\":9.968847352024922118380062310}}";

        public ReportSharedSteps()
        {
            var connectionString = ConfigurationManager.ConnectionStrings[PersistenceNames.PsrsDBConnectionString]
                .ConnectionString;

            _reportRepository = new SQLReportRepository(connectionString);

            _reportDto.Id = new Guid("34C46350-6383-4FB8-823D-8DC189862400");

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
            _reportRepository.Delete(_reportDto.EmployerId);
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
            _reportDto.ReportingData = _validReportJson;
            _reportRepository.Update(_reportDto);
        }

        [Given(@"A valid report has been created")]
        public void GivenAValidReportHasBeenCreated()
        {
            _reportDto.ReportingPeriod = new Period(DateTime.UtcNow).PeriodString;
            _reportDto.ReportingData = _validReportJson;

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
            _reportDto.ReportingData = _invalidReportJson;
            _reportRepository.Update(_reportDto);
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

        [Then(@"the Report Summary page should be displayed")]
        public void ThenTheReportSummaryPageIsDisplayed()
        {
            pageFactory.ReportSummary.Verify();
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

        private string ReadNewlyCreatedReportData()
        {
            var assembly = Assembly.GetExecutingAssembly();

            var resourceName = @"SFA.DAS.PSRService.Web.Specflow.Tests.StepDefinitions.Shared.NewlyCreatedReportData.json";

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
        public void ThenTheYourEmployeesQuestionValuesAndHaveBeenSaved(string p0, string p1, string p2)
        {
            pageFactory.ReportEdit.ClickQuestionLink("Number of employees who work in England");

            var yourEmployees = pageFactory.QuestionYourEmployees;

            yourEmployees.VerifyAtStartValue(p0);
            yourEmployees.VerifyAtEndValue(p1);
            yourEmployees.VerifyNewThisPeriodValue(p2);

            VerifyYourEmployeesValues(p0, p1, p2);
        }

        private void VerifyYourEmployeesValues(string atStart, string atEnd, string newThisPeriod)
        {
            var dto = _reportRepository.Get(_reportDto.ReportingPeriod, _reportDto.EmployerId);

            var jsonObject = JObject.Parse(dto.ReportingData);

            var questions = jsonObject["Questions"]
                    .SingleOrDefault(s => s["Id"].Value<String>() == "ReportNumbers")
                    ["SubSections"].SingleOrDefault(s => s["Id"].Value<String>() == "YourEmployees")
                    ["Questions"]
                    .ToList();

            Assert.AreEqual(atStart, questions.Single(q => q["Id"].Value<String>() == "atStart")["Answer"].Value<String>());
            Assert.AreEqual(atEnd, questions.Single(q => q["Id"].Value<String>() == "atEnd")["Answer"].Value<String>());
            Assert.AreEqual(newThisPeriod, questions.Single(q => q["Id"].Value<String>() == "newThisPeriod")["Answer"].Value<String>());
        }
    }
}
