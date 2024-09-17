using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.Extensions.Logging;
using Moq;
using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.EAS.Account.Api.Types;
using SFA.DAS.PSRService.Web.Models;
using SFA.DAS.PSRService.Web.Services;

namespace SFA.DAS.PSRService.Web.UnitTests.ServiceTests.EmployeerAccountServiceTests.Given_User_Has_One_Account_But_No_Employer_Accounts;

[ExcludeFromCodeCoverage]
public abstract class Given_User_Has_One_Account_But_No_Employer_Accounts : GivenWhenThen<EmployerAccountService>
{
    private Mock<ILogger<EmployerAccountService>> _loggerMock;
    private Mock<IAccountApiClient> _accountApiClientMock;

    protected static readonly string UserId = Guid.NewGuid().ToString();

    private const string AccountIdOne = "MR66J4";
    private static readonly string[] AccountId = [AccountIdOne];

    private readonly IList<TeamMemberViewModel> _teamMembers = null;
    protected static IList<EmployerIdentifier> EmployerIdentifiers => BuildEmployerIdentifierList(AccountId);

    protected override void Given()
    {
        _loggerMock = new Mock<ILogger<EmployerAccountService>>();
        _accountApiClientMock = new Mock<IAccountApiClient>(MockBehavior.Strict);

        _accountApiClientMock.Setup(s => s.GetAccountUsers(AccountIdOne)).ReturnsAsync(_teamMembers);

        Sut = new EmployerAccountService(_loggerMock.Object, _accountApiClientMock.Object);
    }

    private static IList<EmployerIdentifier> BuildEmployerIdentifierList(string[] accountIds)
    {
        return accountIds.Select(EmployerIdentifierWitNoRoleForAccount).ToList();
    }

    private static EmployerIdentifier EmployerIdentifierWitNoRoleForAccount(string accountId)
    {
        return new EmployerIdentifier { AccountId = accountId };
    }
}