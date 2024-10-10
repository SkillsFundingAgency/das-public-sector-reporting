using System;
using System.Collections.Generic;
using MediatR;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Application.ReportHandlers;

public class GetReportEditHistoryMostRecentFirst : IRequest<IEnumerable<AuditRecord>>
{
    public Period Period { get; }
    public  string AccountId{ get; }

    public GetReportEditHistoryMostRecentFirst(Period period, string accountId)
    {
        if (string.IsNullOrWhiteSpace(accountId))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(accountId));

        Period = period;
        AccountId= accountId;
    }
}