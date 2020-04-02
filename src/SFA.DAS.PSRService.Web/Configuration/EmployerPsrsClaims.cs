namespace SFA.DAS.PSRService.Web.Configuration
{
    public static class EmployerPsrsClaims
    {
        public static string IdamsUserIdClaimTypeIdentifier => "http://das/employer/identity/claims/id";
        public static string AccountsClaimsTypeIdentifier => "http://das/employer/identity/claims/associatedAccounts";
        public static string NameClaimsTypeIdentifier => "http://das/employer/identity/claims/display_name";
        public static string EmailClaimsTypeIdentifier => "http://das/employer/identity/claims/email_address";
    }
}
