using System;
using SFA.DAS.EAS.Account.Api.Client;

namespace SFA.DAS.PSRService.Web.Configuration
{
    public interface IWebConfiguration
    {
        IdentityServerConfiguration Identity { get; set; }
        AccountApiConfiguration AccountsApi { get; set; }
        string SqlConnectionString { get; set; }
        DateTime SubmissionClose { get; set; }
        string RootDomainUrl { get; set; }
    }
}