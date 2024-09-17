using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using SFA.DAS.PSRService.Web.Configuration.Authorization;

namespace SFA.DAS.PSRService.Web.UnitTests.ReportControllerTests.Authorization;

[ExcludeFromCodeCoverage]
[Category("Made Obsolete by automation testing")]
public sealed class When_Change_Method_Is_Called : Given_A_ReportController
{
    private Attribute _attribute;

    protected override Task When()
    {
        _attribute = Sut.GetType().GetMethod(nameof(Sut.Change)).GetCustomAttribute(typeof(AuthorizeAttribute));
        return Task.CompletedTask;
    }

    [Test]
    public void Then_Change_Method_Has_Authorization()
    {
        _attribute.Should().NotBeNull();
    }

    [Test]
    public void Then_Change_Method_Is_Authorized_With_CanEdit_Policy()
    {
        ((AuthorizeAttribute)_attribute).Policy.Should().Be(PolicyNames.CanEditReport);
    }
}