using System.Collections.Generic;
using SFA.DAS.PSRService.Web.Models;

namespace SFA.DAS.PSRService.Web.UnitTests.ServiceTests.EmployeerAccountServiceTests.Given_User_Has_No_Accounts;

[ExcludeFromCodeCoverage]
public sealed class When_Roles_Are_Requested : Given_User_Has_No_Accounts
{
    private IEnumerable<EmployerIdentifier> _employerIdentifiers;

    protected override async Task When()
    {
        _employerIdentifiers = await Sut.GetUserRoles(EmployerIdentifiers, UserId);
    }

    [Test]
    public void Then_No_Accounts_Returned()
    {
        _employerIdentifiers.Should().HaveCount(0);
    }
}