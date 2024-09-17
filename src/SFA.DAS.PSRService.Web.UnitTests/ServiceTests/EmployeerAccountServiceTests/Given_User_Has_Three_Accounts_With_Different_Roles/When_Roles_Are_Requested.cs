using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Models;

namespace SFA.DAS.PSRService.Web.UnitTests.ServiceTests.EmployeerAccountServiceTests.Given_User_Has_Three_Accounts_With_Different_Roles;

[ExcludeFromCodeCoverage]
public sealed class When_Roles_Are_Requested : GivenUserHasThreeAccountsWithDifferentRoles
{
    private IEnumerable<EmployerIdentifier> _employerIdentifiers;

    protected override void When()
    {
        _employerIdentifiers = Sut.GetUserRoles(EmployerIdentifiers, UserId).Result;
    }

    [Test]
    public void Then_Three_Accounts_Are_Visible()
    {
        _employerIdentifiers.Should().HaveCount(3);
    }

    [Test]
    public void Then_All_Have_Roles()
    {
        _employerIdentifiers.Where(w => !string.IsNullOrWhiteSpace(w.Role)).Should().HaveCount(3);
    }

    [Test]
    public void Then_AccountOne_Has_Owner_Role()
    {
        _employerIdentifiers.Should().Contain(w => w.Role == EmployerPsrsRoleNames.Owner && w.AccountId == AccountIdOne);
    }

    [Test]
    public void Then_AccountTwo_Has_Transactor_Role()
    {
        _employerIdentifiers.Should().Contain(w => w.Role == EmployerPsrsRoleNames.Transactor && w.AccountId == AccountIdTwo);
    }

    [Test]
    public void Then_AccountThree_Viewer_Role()
    {
        _employerIdentifiers.Should().Contain(w => w.Role == EmployerPsrsRoleNames.Viewer && w.AccountId == AccountIdThree);
    }
}