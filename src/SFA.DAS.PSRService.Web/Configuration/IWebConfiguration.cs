﻿using System;
using SFA.DAS.EAS.Account.Api.Client;

namespace SFA.DAS.PSRService.Web.Configuration
{
    public interface IWebConfiguration
    {
        IdentityServerConfiguration Identity { get; set; }
        AccountApiConfiguration AccountsApi { get; set; }
        string SqlConnectionString { get; set; }
        DateTime SubmissionClose { get; set; }
        string ApplicationUrl { get; set; }
        string RootDomainUrl { get; set; }
        string EmployerAccountsBaseUrl { get; set; }
        string EmployerFinanceBaseUrl { get; set; }
        string EmployerRecruitBaseUrl { get; set; }
        string HomeUrl { get; set; }
        SessionStoreConfiguration SessionStore { get; set; }
        TimeSpan? AuditWindowSize { get; set; }
        ZenDeskConfiguration ZenDeskConfig { get; set; }
    }
}