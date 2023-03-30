using System;
using SFA.DAS.PSRService.Application.ReportHandlers;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Application.UnitTests.ReportHandlerTests
{
    public sealed class UpdateReportRequestBuilder
    {
        private Report _report;

        private Action<UpdateReportRequest> SetUserName = (r) => { };
        private Action<UpdateReportRequest> SetUserId = (r) => { };
        private Action<UpdateReportRequest> SetAuditWindowSize = (r) => { };
        private string _userName = "SomeUser";
        private Guid _userId = new Guid("4CAF1741-AC15-434E-93F3-FBD8CB426B66");

        public UpdateReportRequestBuilder WithReport(Report report)
        {
            _report = report;

            return this;
        }

        public UpdateReportRequestBuilder WithUserName(string userName)
        {
            _userName = userName;

            return this;
        }

        public UpdateReportRequestBuilder WithUserId(Guid userId)
        {
            _userId = userId;

            return this;
        }

        public UpdateReportRequestBuilder WithAutoWindowSizeInMinutes(int mins)
        {
            SetAuditWindowSize = (r) => r.AuditWindowSize = TimeSpan.FromMinutes(mins);

            return this;
        }

        public UpdateReportRequestBuilder WithAutoWindowSizeInSeconds(int secs)
        {
            SetAuditWindowSize = (r) => r.AuditWindowSize = TimeSpan.FromSeconds(secs);

            return this;
        }

        public UpdateReportRequest Build()
        {
            var user = new User
            {
                Name = _userName,
                Id = _userId
            };


            var request = new UpdateReportRequest(_report, user,null);

            SetUserName(request);
            SetUserId(request);
            SetAuditWindowSize(request);

            return request;
        }
    }
}