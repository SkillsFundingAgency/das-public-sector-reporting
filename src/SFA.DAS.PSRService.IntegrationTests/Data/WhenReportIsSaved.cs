using System;
using System.Linq;
using NUnit.Framework;
using SFA.DAS.PSRService.Application.Domain;
using SFA.DAS.PSRService.Data;

namespace SFA.DAS.PSRService.IntegrationTests.Data
{
    [TestFixture]
    public class WhenReportIsSaved
    {
        private ReportRepository _reportRepository;

        [SetUp]
        public void SetUp()
        {
            TestHelper.ClearData();
            _reportRepository = new ReportRepository(TestHelper.ConnectionString);
        }

        [Test]
        public void ThenItShouldAppearInDb()
        {
            // arrange
            var expectedReport = new ReportDto
            {
                Id = Guid.NewGuid(),
                EmployerId = "Uncle Sam",
                ReportingData = "Some genious piece of json",
                ReportingPeriod = "1111",
                Submitted = false
            };

            // act
            _reportRepository.Create(expectedReport);

            // assert
            var actualReports = TestHelper.GetAllReports();
            Assert.AreEqual(1, actualReports.Count);

            var actualReport = actualReports[0];
            CompareReports(expectedReport, actualReport);
        }

        [Test]
        public void ThenItShouldBeAbleToBeReadFromDb()
        {
            // arrange
            var expectedReport = new ReportDto
            {
                Id = Guid.NewGuid(),
                EmployerId = "Uncle Bob",
                ReportingData = "Some dumb piece of json",
                ReportingPeriod = "2222",
                Submitted = true
            };

            // act
            _reportRepository.Create(expectedReport);
            var actualReport = _reportRepository.Get(expectedReport.ReportingPeriod, expectedReport.EmployerId);

            // assert
            Assert.IsNotNull(actualReport);
            CompareReports(expectedReport, actualReport);
        }

        [Test]
        public void ThenItShouldBeAbleToBeReadFromDbAsSubmitted()
        {
            // arrange
            var expectedReport1 = new ReportDto
            {
                Id = Guid.NewGuid(),
                EmployerId = "Uncle Bob",
                ReportingData = "Some dumb piece of json",
                ReportingPeriod = "2222",
                Submitted = true
            };
            var expectedReport2 = new ReportDto
            {
                Id = Guid.NewGuid(),
                EmployerId = "Uncle Bob",
                ReportingData = "Some stupid piece of json",
                ReportingPeriod = "3333",
                Submitted = true
            };
            var unexpectedReport = new ReportDto
            {
                Id = Guid.NewGuid(),
                EmployerId = "Uncle Bob",
                ReportingData = "Some junk piece of json",
                ReportingPeriod = "Noo!",
                Submitted = false
            };

            // act
            _reportRepository.Create(expectedReport1);
            _reportRepository.Create(expectedReport2);
            _reportRepository.Create(unexpectedReport);
            var actualReports = _reportRepository.GetSubmitted(expectedReport1.EmployerId).OrderBy(r => r.ReportingPeriod).ToArray();

            // assert
            Assert.AreEqual(2, actualReports.Length);
            
            CompareReports(expectedReport1, actualReports[0]);
            CompareReports(expectedReport2, actualReports[1]);
        }

        [Test]
        public void ThenItShouldBeAbleToGetUpdated()
        {
            // arrange
            var originalReport = new ReportDto
            {
                Id = Guid.NewGuid(),
                EmployerId = "Uncle Bob",
                ReportingData = "Some dumb piece of json",
                ReportingPeriod = "Rush",
                Submitted = false
            };
            var updatedReport = new ReportDto
            {
                Id = originalReport.Id,
                EmployerId = "Uncle Bob",
                ReportingData = "Some stupid piece of json",
                ReportingPeriod = "Rush",
                Submitted = true
            };

            // act
            _reportRepository.Create(originalReport);
            var actualReport = _reportRepository.Get(originalReport.ReportingPeriod, originalReport.EmployerId);
            CompareReports(originalReport, actualReport);
            _reportRepository.Update(updatedReport);

            // assert
            actualReport = _reportRepository.Get(originalReport.ReportingPeriod, originalReport.EmployerId);
            CompareReports(updatedReport, actualReport);
        }

        private static void CompareReports(ReportDto expectedReport, ReportDto actualReport)
        {
            Assert.AreEqual(expectedReport.Id, actualReport.Id);
            Assert.AreEqual(expectedReport.EmployerId, actualReport.EmployerId);
            Assert.AreEqual(expectedReport.ReportingData, actualReport.ReportingData);
            Assert.AreEqual(expectedReport.ReportingPeriod, actualReport.ReportingPeriod);
            Assert.AreEqual(expectedReport.Submitted, actualReport.Submitted);
        }
    }
}
