using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.PSRService.Web.Models;

namespace SFA.DAS.PSRService.Web.UnitTests.ServiceTests.EmployeerAccountServiceTests.Given_User_Has_One_Account_But_Not_A_TeamMember;

[ExcludeFromCodeCoverage]
public sealed class When_Roles_Are_Requested : Given_User_Has_One_Account_But_Not_A_TeamMember
{
    private IEnumerable<EmployerIdentifier> _employerIdentifiers;

    protected override void When()
    {
        _employerIdentifiers = Sut.GetUserRoles(EmployerIdentifiers, UserId).Result;
    }

    [Test]
    public void Then_No_Accounts_Returned()
    {
        _employerIdentifiers.Should().HaveCount(0);
    }

}