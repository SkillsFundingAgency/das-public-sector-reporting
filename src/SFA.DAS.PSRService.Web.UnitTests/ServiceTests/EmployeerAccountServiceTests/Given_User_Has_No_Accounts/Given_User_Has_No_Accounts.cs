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

namespace SFA.DAS.PSRService.Web.UnitTests.ServiceTests.EmployeerAccountServiceTests.Given_User_Has_No_Accounts
{
    [ExcludeFromCodeCoverage]
    public abstract class Given_User_Has_No_Accounts : GivenWhenThen<EmployerAccountService>
    {
        private Mock<ILogger<EmployerAccountService>> _loggerMock;
        private Mock<IAccountApiClient> _accountApiClienMock;

        protected static string UserId = Guid.NewGuid().ToString();

        protected static readonly string _accountIdOne = "MR66J4";
        protected static readonly string _accountIdTwo = "JD83N5";
        private static readonly string[] AccountId = { _accountIdOne, _accountIdTwo };

        private static readonly TeamMemberViewModel _teamMemberOwner = new TeamMemberViewModel() {Role = EmployerPsrsRoleNames.Owner, UserRef = UserId};
        private static readonly TeamMemberViewModel _teamMemberTransactor = new TeamMemberViewModel() { Role = EmployerPsrsRoleNames.Transactor, UserRef = UserId};
        private static readonly TeamMemberViewModel _teamMemberViewer = new TeamMemberViewModel() { Role = EmployerPsrsRoleNames.Viewer, UserRef = UserId};

        private readonly IList<TeamMemberViewModel> _teamMembersOwner = new List<TeamMemberViewModel>() { _teamMemberOwner };
        private readonly IList<TeamMemberViewModel> _teamMembersTransactor = new List<TeamMemberViewModel>() { _teamMemberTransactor };
        private readonly IList<TeamMemberViewModel> _teamMembersViewer = new List<TeamMemberViewModel>() { _teamMemberViewer };

        protected IList<EmployerIdentifier> EmployerIdentifiers => BuildEmployerIdentifierList(AccountId);

        protected override void Given()
        {

            _loggerMock = new Mock<ILogger<EmployerAccountService>>();
            _accountApiClienMock = new Mock<IAccountApiClient>(MockBehavior.Strict);

            _accountApiClienMock.Setup(s => s.GetAccountUsers(_accountIdOne)).ReturnsAsync((IList<TeamMemberViewModel>)null);
            _accountApiClienMock.Setup(s => s.GetAccountUsers(_accountIdTwo)).ReturnsAsync(new List<TeamMemberViewModel>());

            SUT = new EmployerAccountService(_loggerMock.Object,_accountApiClienMock.Object);

            
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
}