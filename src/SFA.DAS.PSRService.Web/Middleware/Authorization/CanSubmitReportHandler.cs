using Microsoft.AspNetCore.Authorization;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Configuration.Authorization;
using SFA.DAS.PSRService.Web.Models;

namespace SFA.DAS.PSRService.Web.Middleware.Authorization;

public class CanSubmitReportHandler : AccountsClaimsAuthorizationHandler<CanSubmitReport>
{
    protected override Task AuthorizeRequirementAgainstCurrentAccountIdEmployerIdentifierInformation(
        AuthorizationHandlerContext context,
        CanSubmitReport requirement, 
        EmployerIdentifier employerIdentifier)
    {
        if (employerIdentifier.Role.Equals(EmployerPsrsRoleNames.Owner, StringComparison.OrdinalIgnoreCase))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}