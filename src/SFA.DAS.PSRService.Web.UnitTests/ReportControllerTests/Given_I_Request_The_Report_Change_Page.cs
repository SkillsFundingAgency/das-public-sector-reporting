using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Web.ViewModels;

namespace SFA.DAS.PSRService.Web.UnitTests.ReportControllerTests
{
    [TestFixture]
    public class Given_I_Request_The_Report_Change_Page : ReportControllerTestBase
    {
        [Test]
        public void And_The_Report_Exists_Then_Report_Is_Saved()
        {
            // arrange
            var report = new Report();
            _mockReportService.Setup(s => s.IsSubmitValid(It.IsAny<Report>())).Returns(true);
            _mockReportService.Setup(s => s.GetCurrentReportPeriod()).Returns("1617");
            _mockReportService.Setup(s => s.GetReport(It.IsAny<string>(), It.IsAny<string>())).Returns(report);


            var organisationVm = new OrganisationViewModel()
            {
                Report = report
            };
            var result = _controller.Change(organisationVm);

            Assert.AreEqual(typeof(ViewResult), result.GetType());
            var editViewResult = result as ViewResult;
            Assert.IsNotNull(editViewResult);

            _mockReportService.Verify(x => x.SaveReport(report));
        }
    }
}
