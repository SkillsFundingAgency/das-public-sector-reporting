using System.Security.Claims;

namespace SFA.DAS.PSRService.IntegrationTests.Web;

public class TestPrincipal : ClaimsPrincipal
{
    public override Claim FindFirst(string type)
    {
        return new Claim(type, Guid.Empty.ToString());
    }
}