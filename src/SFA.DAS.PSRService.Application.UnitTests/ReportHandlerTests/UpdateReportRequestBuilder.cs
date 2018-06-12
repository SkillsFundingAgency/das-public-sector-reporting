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

        public UpdateReportRequestBuilder WithReport(Report report)
        {
            _report = report;

            return this;
        }

        public UpdateReportRequestBuilder WithUserName(string userName)
        {
            SetUserName = (r) => r.UserName = userName;

            return this;
        }

        public UpdateReportRequestBuilder WithUserId(Guid userId)
        {
            SetUserId = (r) => r.UserId = userId;

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
            var request = new UpdateReportRequest(_report);

            SetUserName(request);
            SetUserId(request);
            SetAuditWindowSize(request);

            return request;
        }
    }
}