using System.Security.Claims;
using SFA.DAS.PSRService.Web.Configuration;

namespace SFA.DAS.PSRService.Web.Models;

public class UserModel
{
    public string Email { get; init; }
    public string DisplayName { get; init; }
    public Guid Id { get; init; }
    
    public static UserModel From(ClaimsPrincipal identity)
    {
        if (identity == null)
        {
            return new UserModel();
        }

        return new UserModel
        {
            Email = identity.FindFirst(EmployerPsrsClaims.EmailClaimsTypeIdentifier)?.Value,
            DisplayName = identity.FindFirst(EmployerPsrsClaims.NameClaimsTypeIdentifier)?.Value,
            Id = Guid.Parse(identity.FindFirst(EmployerPsrsClaims.IdamsUserIdClaimTypeIdentifier)?.Value)
        };
    }
}