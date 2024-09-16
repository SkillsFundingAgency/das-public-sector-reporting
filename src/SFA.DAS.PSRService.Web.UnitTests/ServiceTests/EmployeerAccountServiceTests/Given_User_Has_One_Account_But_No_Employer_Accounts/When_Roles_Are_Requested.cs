using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Models;

namespace SFA.DAS.PSRService.Web.UnitTests.ServiceTests.EmployeerAccountServiceTests.Given_User_Has_One_Account_But_No_Employer_Accounts;

[ExcludeFromCodeCoverage]
public sealed class When_Roles_Are_Requested : Given_User_Has_One_Account_But_No_Employer_Accounts
{
    private IEnumerable<EmployerIdentifier> _employerIdentifiers;

    protected override void When()
    {
        _employerIdentifiers = SUT.GetUserRoles(EmployerIdentifiers, UserId).Result;
    }

    [Test]
    public void Then_No_Accounts_Returned()
    {
        _employerIdentifiers.Should().HaveCount(0);
    }

}