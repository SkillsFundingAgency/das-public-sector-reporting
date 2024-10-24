using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.EAS.Account.Api.Types;
using SFA.DAS.PSRService.Web.Models;
using SFA.DAS.PSRService.Web.Services;

namespace SFA.DAS.PSRService.Web.UnitTests.ServiceTests.EmployeerAccountServiceTests.Given_User_Has_One_Account_But_Not_A_TeamMember;

[ExcludeFromCodeCoverage]
public abstract class Given_User_Has_One_Account_But_Not_A_TeamMember : GivenWhenThen<EmployerAccountService>
{
    private Mock<ILogger<EmployerAccountService>> _loggerMock;
    private Mock<IAccountApiClient> _accountApiClientMock;

    protected static readonly string UserId = Guid.NewGuid().ToString();

    private const string AccountIdOne = "MR66J4";
    private static readonly string[] AccountId = [AccountIdOne];

    private readonly List<TeamMemberViewModel> _teamMembers = [new(), new()];
    protected static List<EmployerIdentifier> EmployerIdentifiers => BuildEmployerIdentifierList(AccountId);

    protected override void Given()
    {
        _loggerMock = new Mock<ILogger<EmployerAccountService>>();
        _accountApiClientMock = new Mock<IAccountApiClient>(MockBehavior.Strict);
        _accountApiClientMock.Setup(s => s.GetAccountUsers(AccountIdOne)).ReturnsAsync(_teamMembers);

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