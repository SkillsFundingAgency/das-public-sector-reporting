using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SFA.DAS.PSRService.Web.Middleware.Authorization;
using SFA.DAS.PSRService.Web.Models;

namespace SFA.DAS.PSRService.Web.UnitTests.Middleware.RoleRequierments;

public class StubUserHasRoleForAccount : 
    AccountsClaimsAuthorizationHandler<TestRequirement>
{
    public static string RequiredRole => "Role";

    protected override Task AuthorizeRequirementAgainstCurrentAccountIdEmployerIdentifierInformation(
        AuthorizationHandlerContext context, 
        TestRequirement requirement, 
        EmployerIdentifier employerIdentifier)
    {
        throw new System.NotImplementedException();
    }
}