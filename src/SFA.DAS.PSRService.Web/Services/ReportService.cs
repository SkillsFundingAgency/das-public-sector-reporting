using MediatR;
using SFA.DAS.PSRService.Application.ReportHandlers;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Models;

namespace SFA.DAS.PSRService.Web.Services;

public class ReportService(IWebConfiguration config, IMediator mediator, IPeriodService periodService) : IReportService
{
    public async Task CreateReport(string employerId, UserModel user, bool? isLocalAuthority)
    {
        var currentPeriod = periodService.GetCurrentPeriod();

        var requestUser = new User
        {
            Name = user.DisplayName,
            Id = user.Id
        };

        var request = new CreateReportRequest(
            requestUser,
            employerId,
            currentPeriod.PeriodString,
            isLocalAuthority);

        var report = await mediator.Send(request);


        if (report?.Id == null)
        {
            throw new Exception("Unable to create a new report");
        }
    }

    public async Task<Report> GetReport(string period, string employerId)
    {
        var request = new GetReportRequest { Period = period, EmployerId = employerId };
        return await mediator.Send(request);
    }

    public async Task SubmitReport(Report report)
    {
        if (!CanBeEdited(report) || !report.IsValidForSubmission())
        {
            throw new InvalidOperationException("Report is invalid for submission.");
        }

        await mediator.Send(new SubmitReportRequest(report));
    }

    public async Task<IEnumerable<Report>> GetSubmittedReports(string employerId)
    {
        var request = new GetSubmittedRequest { EmployerId = employerId };

        return await mediator.Send(request);
    }

    public async Task SaveReport(Report report, UserModel userModel, bool? isLocalAuthority)
    {
        var user = new User
        {
            Name = userModel.DisplayName,
            Id = userModel.Id
        };

        var request = new UpdateReportRequest(report, user, isLocalAuthority);

        if (config.AuditWindowSize.HasValue)
        {
            request.AuditWindowSize = config.AuditWindowSize.Value;
        }

        await mediator.Send(request);
    }

    public bool CanBeEdited(Report report)
    {
        return report != null
               && !report.Submitted
               && periodService.PeriodIsCurrent(report.Period);
    }

    public async Task<IEnumerable<AuditRecord>> GetReportEditHistoryMostRecentFirst(Period period, string employerId)
    {
        var request = new GetReportEditHistoryMostRecentFirst(period, employerId);

        return await mediator.Send(request);
    }
}