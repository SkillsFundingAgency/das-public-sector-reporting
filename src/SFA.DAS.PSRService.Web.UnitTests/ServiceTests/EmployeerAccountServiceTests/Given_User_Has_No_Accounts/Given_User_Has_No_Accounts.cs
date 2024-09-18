using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.EAS.Account.Api.Types;
using SFA.DAS.PSRService.Web.Models;
using SFA.DAS.PSRService.Web.Services;

namespace SFA.DAS.PSRService.Web.UnitTests.ServiceTests.EmployeerAccountServiceTests.Given_User_Has_No_Accounts;

[ExcludeFromCodeCoverage]
public abstract class Given_User_Has_No_Accounts : GivenWhenThen<EmployerAccountService>
{
    protected static readonly string UserId = Guid.NewGuid().ToString();

    private Mock<ILogger<EmployerAccountService>> _loggerMock;
    private Mock<IAccountApiClient> _accountApiClientMock;

    private const string AccountIdOne = "MR66J4";
    private const string AccountIdTwo = "JD83N5";

    private static readonly string[] AccountId = [AccountIdOne, AccountIdTwo];

    protected static IList<EmployerIdentifier> EmployerIdentifiers => BuildEmployerIdentifierList(AccountId);

    protected override void Given()
    {
        _loggerMock = new Mock<ILogger<EmployerAccountService>>();
        _accountApiClientMock = new Mock<IAccountApiClient>(MockBehavior.Strict);

        _accountApiClientMock.Setup(s => s.GetAccountUsers(AccountIdOne)).ReturnsAsync((IList<TeamMemberViewModel>)null);
        _accountApiClientMock.Setup(s => s.GetAccountUsers(AccountIdTwo)).ReturnsAsync(new List<TeamMemberViewModel>());

        Sut = new EmployerAccountService(_loggerMock.Object, _accountApiClientMock.Object);
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