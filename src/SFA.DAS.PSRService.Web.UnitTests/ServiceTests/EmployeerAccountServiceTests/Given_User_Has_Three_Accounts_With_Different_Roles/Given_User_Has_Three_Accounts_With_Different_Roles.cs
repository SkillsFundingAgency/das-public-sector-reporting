using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Moq;
using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.EAS.Account.Api.Types;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Models;
using SFA.DAS.PSRService.Web.Services;

namespace SFA.DAS.PSRService.Web.UnitTests.ServiceTests.EmployeerAccountServiceTests.Given_User_Has_Three_Accounts_With_Different_Roles;

[ExcludeFromCodeCoverage]
public abstract class GivenUserHasThreeAccountsWithDifferentRoles : GivenWhenThen<EmployerAccountService>
{
    private Mock<ILogger<EmployerAccountService>> _loggerMock;
    private Mock<IAccountApiClient> _accountApiClienMock;

    protected static string UserId = Guid.NewGuid().ToString();

    protected static readonly string _accountIdOne = "MR66J4";
    protected static readonly string _accountIdTwo = "JD83N5";
    protected static readonly string _accountIdThree = "WD34D1";
    private static readonly string[] AccountId = [_accountIdOne, _accountIdTwo, _accountIdThree];

    private static readonly TeamMemberViewModel _teamMemberOwner = new() {Role = EmployerPsrsRoleNames.Owner, UserRef = UserId};
    private static readonly TeamMemberViewModel _teamMemberTransactor = new() { Role = EmployerPsrsRoleNames.Transactor, UserRef = UserId};
    private static readonly TeamMemberViewModel _teamMemberViewer = new() { Role = EmployerPsrsRoleNames.Viewer, UserRef = UserId};

    private readonly IList<TeamMemberViewModel> _teamMembersOwner = new List<TeamMemberViewModel>() { _teamMemberOwner };
    private readonly IList<TeamMemberViewModel> _teamMembersTransactor = new List<TeamMemberViewModel>() { _teamMemberTransactor };
    private readonly IList<TeamMemberViewModel> _teamMembersViewer = new List<TeamMemberViewModel>() { _teamMemberViewer };

    protected IList<EmployerIdentifier> EmployerIdentifiers => BuildEmployerIdentifierList(AccountId);

    private readonly static IList<AccountDetailViewModel> accountDetailViewModel = new List<AccountDetailViewModel>() { new() { HashedAccountId = _accountIdOne}, new() { HashedAccountId = _accountIdTwo }, new() { HashedAccountId = _accountIdThree } };

    protected override void Given()
    {

        _loggerMock = new Mock<ILogger<EmployerAccountService>>();
        _accountApiClienMock = new Mock<IAccountApiClient>(MockBehavior.Strict);

        _accountApiClienMock.Setup(s => s.GetAccountUsers(_accountIdOne)).ReturnsAsync(_teamMembersOwner);
        _accountApiClienMock.Setup(s => s.GetAccountUsers(_accountIdTwo)).ReturnsAsync(_teamMembersTransactor);
        _accountApiClienMock.Setup(s => s.GetAccountUsers(_accountIdThree)).ReturnsAsync(_teamMembersViewer);

        _accountApiClienMock.Setup(s => s.GetUserAccounts(UserId)).ReturnsAsync(accountDetailViewModel);

        Sut = new EmployerAccountService(_loggerMock.Object,_accountApiClienMock.Object);

            
    }

    private IList<EmployerIdentifier> BuildEmployerIdentifierList(string[] accountIds)
    {
        var employerList = new List<EmployerIdentifier>();

        foreach (string accountId in accountIds)
        {
            employerList.Add(EmployerIdentifierWitNoRoleForAccount(accountId));
        }

        return employerList;
    }

    private EmployerIdentifier EmployerIdentifierWitNoRoleForAccount(string accountId)
    {
        return
            new EmployerIdentifier()
            {
                AccountId = accountId
            };
    }

}