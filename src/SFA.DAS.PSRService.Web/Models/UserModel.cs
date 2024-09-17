using System.Security.Claims;
using SFA.DAS.PSRService.Web.Configuration;

namespace SFA.DAS.PSRService.Web.Models;

public class UserModel
{
    public string Email { get; set; }
    public string DisplayName { get; set; }
    public Guid Id { get; set; }

    public UserModel() { }
    
    public UserModel(ClaimsPrincipal identity)
    {
        Email = identity.FindFirst( EmployerPsrsClaims.EmailClaimsTypeIdentifier)?.Value;
        DisplayName = identity.FindFirst(EmployerPsrsClaims.NameClaimsTypeIdentifier)?.Value;
        Id = Guid.Parse( identity.FindFirst(EmployerPsrsClaims.IdamsUserIdClaimTypeIdentifier)?.Value);
    }
}