using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace SFA.DAS.PSRService.Web.Middleware.Authorization
{
    public abstract class UserHasRoleForAccount<TypeOfRequirement>
        : AuthorizationHandler<TypeOfRequirement>
        where TypeOfRequirement : IAuthorizationRequirement
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            TypeOfRequirement requirement)
        {
            throw new System.NotImplementedException();
        }
    }
}