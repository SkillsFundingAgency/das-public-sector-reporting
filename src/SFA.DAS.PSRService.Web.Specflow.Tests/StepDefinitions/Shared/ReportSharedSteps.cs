﻿using System;
using System.Configuration;
using System.IO;
using System.Reflection;
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
        public void ThenTheYourEmployeesQuestionValuesAndHaveBeenSaved(string atStart, string atEnd, string newThisPeriod)
        {
            pageFactory.ReportEdit.ClickQuestionLink("Number of employees who work in England");

            var yourEmployees = pageFactory.QuestionYourEmployees;

            yourEmployees.VerifyAtStartValue(atStart);
            yourEmployees.VerifyAtEndValue(atEnd);
            yourEmployees.VerifyNewThisPeriodValue(newThisPeriod);

            VerifyYourEmployeesAtStartHasBeenPersisted(atStart);
            VerifyYourEmployeesAtEndHasBeenPersisted(atEnd);
            VerifyYourEmployeesNewThisPeriodHasBeenPersisted(newThisPeriod);
        }

        private void VerifyYourEmployeesNewThisPeriodHasBeenPersisted(string newThisPeriod)
        {
            _reportRepository
                .VerifyForReportId(_reportDto.Id)
                .YourEmployees
                .NewThisPeriod
                .HasAnswer(newThisPeriod);
        }

        private void VerifyYourEmployeesAtEndHasBeenPersisted(string atEnd)
        {
            _reportRepository
                .VerifyForReportId(_reportDto.Id)
                .YourEmployees
                .AtEnd
                .HasAnswer(atEnd);
        }

        private void VerifyYourEmployeesAtStartHasBeenPersisted(string atStart)
        {
            _reportRepository
                .VerifyForReportId(_reportDto.Id)
                .YourEmployees
                .AtStart
                .HasAnswer(atStart);
        }
    }
}
