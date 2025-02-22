﻿using System;
using SFA.DAS.PSRService.Application.ReportHandlers;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Application.UnitTests.ReportHandlerTests;

public sealed class CreateReportRequestBuilder
{
    private string _userName = "SomeUser";
    private Guid _userId = new("FCCD9CDE-5E50-4AA2-8722-90EBB8E1E7F4");
    private string _employerId;
    private string _period = "1718";
    private bool _isLocalAuthority ;

    public CreateReportRequestBuilder WithUserName(string userName)
    {
        _userName = userName;

        return this;
    }
    public CreateReportRequestBuilder WithIsLocalAuthority(bool isLocalAuthority)
    {
        _isLocalAuthority = isLocalAuthority;

        return this;
    }

    public CreateReportRequestBuilder WithUserId(Guid userId)
    {
        _userId = userId;

        return this;
    }

    public CreateReportRequestBuilder ForPeriod(string period)
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
            _period, _isLocalAuthority);

        return request;
    }
}