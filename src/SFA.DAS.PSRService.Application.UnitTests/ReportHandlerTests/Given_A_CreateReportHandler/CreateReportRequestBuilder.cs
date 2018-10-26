using System;
using SFA.DAS.PSRService.Application.ReportHandlers;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Application.UnitTests.ReportHandlerTests.Given_A_CreateReportHandler
{
    public sealed class CreateReportRequestBuilder
    {
        private string _userName = "SomeUser";
        private Guid _userId = new Guid("FCCD9CDE-5E50-4AA2-8722-90EBB8E1E7F4");
        private string _employerId;
        private Period _period = Period.ParsePeriodString("1718");

        public CreateReportRequestBuilder WithUserName(string userName)
        {
            _userName = userName;

            return this;
        }

        public CreateReportRequestBuilder WithUserId(Guid userId)
        {
            _userId = userId;

            return this;
        }

        public CreateReportRequestBuilder ForPeriod(Period period)
        {
            _period = period;

            return this;
        }

        public CreateReportRequestBuilder WithEmployerId(string employerId)
        {
            _employerId = employerId;

            return this;
        }


        public CreateReportRequest Build()
        {
                var user = new User
                {
                    Name = _userName,
                    Id = _userId
                };

            var request = new CreateReportRequest(
                user, 
                _employerId,
                _period);

            return request;
        }
    }
}