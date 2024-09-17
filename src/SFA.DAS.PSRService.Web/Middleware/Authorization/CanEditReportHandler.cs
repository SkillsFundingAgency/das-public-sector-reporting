using Microsoft.AspNetCore.Authorization;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Configuration.Authorization;
using SFA.DAS.PSRService.Web.Models;

namespace SFA.DAS.PSRService.Web.Middleware.Authorization
{
    public class CanEditReportHandler : AccountsClaimsAuthorizationHandler<CanEditReport>
    {
        protected override Task AuthorizeRequirementAgainstCurrentAccountIdEmployerIdentifierInformation(
            AuthorizationHandlerContext context,
            CanEditReport requirement,
            EmployerIdentifier employerIdentifier)
        {
            if (employerIdentifier.Role.Equals(EmployerPsrsRoleNames.Owner, StringComparison.OrdinalIgnoreCase) ||
                employerIdentifier.Role.Equals(EmployerPsrsRoleNames.Transactor, StringComparison.OrdinalIgnoreCase))
            {
                context.Succeed(requirement);
            }
                

            return Task.CompletedTask;
        }
    }
}