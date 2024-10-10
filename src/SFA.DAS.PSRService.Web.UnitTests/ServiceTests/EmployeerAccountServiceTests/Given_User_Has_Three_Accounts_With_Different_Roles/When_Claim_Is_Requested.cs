using System.Collections.Generic;
using System.Security.Claims;
using Newtonsoft.Json;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Models;

namespace SFA.DAS.PSRService.Web.UnitTests.ServiceTests.EmployeerAccountServiceTests.Given_User_Has_Three_Accounts_With_Different_Roles;

[ExcludeFromCodeCoverage]
public sealed class When_Claim_Is_Requested : GivenUserHasThreeAccountsWithDifferentRoles
{
    private Claim _claim;

    protected override async Task When()
    {
        _claim = await Sut.GetClaim(UserId);
    }

    [Test]
    public void Then_Name_Should_Be_AssociatedAccounts()
    {
        _claim.Type.Should().Be(EmployerPsrsClaims.AccountsClaimsTypeIdentifier);
    }

    [Test]
    public void Then_Value_Should_Be_Populated()
    {
        _claim.Value.Should().NotBeNullOrWhiteSpace();
    }

    [Test]
    public void Then_Value_Should_Be_Json_Of_Employer_Identifiers()
    {
        var identifiers = JsonConvert.DeserializeObject<Dictionary<string, EmployerIdentifier>>(_claim.Value);

        identifiers.Values.Should().HaveCount(EmployerIdentifiers.Count);
    }
}