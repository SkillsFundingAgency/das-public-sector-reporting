using System.IO;

namespace SFA.DAS.PSRService.Web.Configuration
{
    public static class EmployerPsrsClaims
    {
        public static string IdamsUserIdClaimTypeIdentifier => "http://das/employer/identity/claims/id";
        public static string AccountsClaimsTypeIdentifier => "http://das/employer/identity/claims/associatedAccounts";
    }
}
