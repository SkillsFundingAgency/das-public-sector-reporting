﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using AutoMapper;
using Moq;
using NUnit.Framework;
using SFA.DAS.PSRService.Application.Domain;
using SFA.DAS.PSRService.Application.Interfaces;
using SFA.DAS.PSRService.Application.ReportHandlers;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Application.UnitTests.ReportHandlerTests.GetSubmittedTests
{
    [TestFixture]
    public class Given_I_Get_Submitted_Reports
    {
        private Mock<IMapper> _mapperMock;
        private Mock<IReportRepository> _reportRepositoryMock;
        private IEnumerable<ReportDto> _reportDtoList;
        private IEnumerable<Report> _reportList;

        private GetSubmittedHandler _getSubmittedHandler;

        [SetUp]
        public void Setup()
        {
            _mapperMock = new Mock<IMapper>(MockBehavior.Strict);
            _reportRepositoryMock = new Mock<IReportRepository>(MockBehavior.Strict);
            _getSubmittedHandler = new GetSubmittedHandler(_reportRepositoryMock.Object, _mapperMock.Object);


            _mapperMock.Setup(s => s.Map<Report>(It.IsAny<ReportDto>())).Returns(new Report());

            _reportList = (new List<Report>()
            {
                new Report()
                {
                    Id = Guid.NewGuid(),
                    ReportingPeriod = "1718",
                    EmployerId = 12345,
                    Submitted = false
                },
                new Report()
                {
                    Id = Guid.NewGuid(),
                    ReportingPeriod = "1617",
                    EmployerId = 12345,
                    Submitted = true
                },
                new Report()
                {
                    Id = Guid.NewGuid(),
                    ReportingPeriod = "1516",
                    EmployerId = 12345,
                    Submitted = true
                },
                new Report()
                {
                    Id = Guid.NewGuid(),
                    ReportingPeriod = "1718",
                    EmployerId = 56789,
                    Submitted = false
                }
            }).AsEnumerable();
            _reportDtoList = (new List<ReportDto>()
            {
                new ReportDto()
                {
                    Id = Guid.NewGuid(),
                    ReportingPeriod = "1718",
                    EmployerId = 12345,
                    Submitted = false
                },
                new ReportDto()
                {
                    Id = Guid.NewGuid(),
                    ReportingPeriod = "1617",
                    EmployerId = 12345,
                    Submitted = true
                },
                new ReportDto()
                {
                    Id = Guid.NewGuid(),
                    ReportingPeriod = "1516",
                    EmployerId = 12345,
                    Submitted = true
                },
                new ReportDto()
                {
                    Id = Guid.NewGuid(),
                    ReportingPeriod = "1718",
                    EmployerId = 56789,
                    Submitted = false
                }
            }).AsEnumerable();

        }



        [Test]
        public void When_An_Employer_Id_Isnt_Supplied_Then_Return_Empty_Collection()
        {

            //arrange
            _reportRepositoryMock.Setup(s => s.GetSubmitted(It.IsAny<long>())).Returns((IEnumerable<ReportDto>)null);

            var getSubmittedRequest = new GetSubmittedRequest() { EmployerId = 0 };

            //Act

            var result = _getSubmittedHandler.Handle(getSubmittedRequest, new CancellationToken()).Result;



            //Assert

            CollectionAssert.IsEmpty(result);


        }

        [Test]
        public void When_An_Employer_Id_Has_Submitted_Reports_Then_Return_List()
        {
            //arrange
            _reportRepositoryMock.Setup(s => s.GetSubmitted(It.IsAny<long>())).Returns(_reportDtoList.Where(w => w.Submitted == true && w.EmployerId == 12345));
            var getSubmittedRequest = new GetSubmittedRequest() { EmployerId = 12345 };


            //Act
            var result = _getSubmittedHandler.Handle(getSubmittedRequest, new CancellationToken()).Result;


            //Assert

            Assert.IsNotNull(result);
            var reports = result.ToList();
            CollectionAssert.AllItemsAreInstancesOfType(reports, typeof(Report));
            Assert.AreEqual(reports.Count(), 2);


        }

        [Test]
        public void When_An_Employer_Id_Has_No_Submitted_Reports_Then_Return_Empty_Collection()
        {
            //arrange
            _reportRepositoryMock.Setup(s => s.GetSubmitted(It.IsAny<long>())).Returns((IEnumerable<ReportDto>)null);

            var getSubmittedRequest = new GetSubmittedRequest() { EmployerId = 6768976789 };

            //Act

            var result = _getSubmittedHandler.Handle(getSubmittedRequest, new CancellationToken()).Result;



            //Assert

            CollectionAssert.IsEmpty(result);
        }

    }
}
