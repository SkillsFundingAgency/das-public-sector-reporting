using System;
using System.Configuration;
using SFA.DAS.PSRService.Web.Specflow.Tests.consts;
using SFA.DAS.PSRService.Web.Specflow.Tests.Pages;
using SFA.DAS.PSRService.Web.Specflow.Tests.Repository;
using SFA.DAS.PSRService.Web.Specflow.Tests.TestSupport;
using TechTalk.SpecFlow;


namespace SFA.DAS.PSRService.Web.Specflow.Tests.StepDefinitions
{
    [Binding]
    public class ReportSharedSteps : BaseTest
    {

        private SQLReportRepository _reportRepository;

        private ReportDto _reportDto;

        private string _validReportJson;
        private string _invalidReportJson;
        public ReportSharedSteps()
        {
            var connectionString = ConfigurationManager.ConnectionStrings[PersistenceNames.PsrsDBConnectionString]
                .ConnectionString;

            _reportRepository = new SQLReportRepository(connectionString);

            _reportDto.Id = Guid.Parse("00000000-0000-0000-0000-000000000000");
        }

        [Given(@"No current report exists")]
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

        [Given(@"the report hasnt been submitted")]
        public void GivenTheReportHasntBeenSubmitted()
        {
            _reportDto.Submitted = false;
            _reportRepository.Update(_reportDto);
        }

        [Given(@"I have a valid report")]
        public void GivenIHaveAValidReport()
        {
            _reportDto.ReportingData = _validReportJson;
            _reportRepository.Update(_reportDto);
        }

        [Given(@"the report has been submitted")]
        public void GivenTheReportHasBeenSubmitted()
        {
            _reportDto.Submitted = true;
            _reportRepository.Update(_reportDto);
        }

        [Given(@"I have an invalid report")]
        public void GivenIHaveAnInvalidReport()
        {
            _reportDto.ReportingData = _invalidReportJson;
            _reportRepository.Update(_reportDto);
        }





    }
}
