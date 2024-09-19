using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
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
    private Mock<IAccountApiClient> _accountApiClientMock;

    protected static readonly string UserId = Guid.NewGuid().ToString();

    protected const string AccountIdOne = "MR66J4";
    protected const string AccountIdTwo = "JD83N5";
    protected const string AccountIdThree = "WD34D1";
    private static readonly string[] AccountId = [AccountIdOne, AccountIdTwo, AccountIdThree];

    private static readonly TeamMemberViewModel TeamMemberOwner = new() { Role = EmployerPsrsRoleNames.Owner, UserRef = UserId };
    private static readonly TeamMemberViewModel TeamMemberTransactor = new() { Role = EmployerPsrsRoleNames.Transactor, UserRef = UserId };
    private static readonly TeamMemberViewModel TeamMemberViewer = new() { Role = EmployerPsrsRoleNames.Viewer, UserRef = UserId };

    private readonly List<TeamMemberViewModel> _teamMembersOwner = new List<TeamMemberViewModel> { TeamMemberOwner };
    private readonly List<TeamMemberViewModel> _teamMembersTransactor = new List<TeamMemberViewModel> { TeamMemberTransactor };
    private readonly List<TeamMemberViewModel> _teamMembersViewer = new List<TeamMemberViewModel> { TeamMemberViewer };

    protected static List<EmployerIdentifier> EmployerIdentifiers => BuildEmployerIdentifierList(AccountId);

    private static readonly List<AccountDetailViewModel> AccountDetailViewModel =
    [
        new() { HashedAccountId = AccountIdOne },
        new() { HashedAccountId = AccountIdTwo },
        new() { HashedAccountId = AccountIdThree }
    ];

    protected override void Given()
    {
        _loggerMock = new Mock<ILogger<EmployerAccountService>>();
        _accountApiClientMock = new Mock<IAccountApiClient>(MockBehavior.Strict);

        _accountApiClientMock.Setup(s => s.GetAccountUsers(AccountIdOne)).ReturnsAsync(_teamMembersOwner);
        _accountApiClientMock.Setup(s => s.GetAccountUsers(AccountIdTwo)).ReturnsAsync(_teamMembersTransactor);
        _accountApiClientMock.Setup(s => s.GetAccountUsers(AccountIdThree)).ReturnsAsync(_teamMembersViewer);

        _accountApiClientMock.Setup(s => s.GetUserAccounts(UserId)).ReturnsAsync(AccountDetailViewModel);

        Sut = new EmployerAccountService(_accountApiClientMock.Object);
    }

    private static List<EmployerIdentifier> BuildEmployerIdentifierList(string[] accountIds)
    {
        return accountIds.Select(EmployerIdentifierWitNoRoleForAccount).ToList();
    }

    private static EmployerIdentifier EmployerIdentifierWitNoRoleForAccount(string accountId)
    {
        return new EmployerIdentifier { AccountId = accountId };
    }
}