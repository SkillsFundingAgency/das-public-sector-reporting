using System;
using System.Security.Claims;

namespace SFA.DAS.PSRService.Web.Models
{
    public class UserModel
    {
        public string Email { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string DisplayName { get; set; }
        public Guid Id { get; set; }

        public UserModel() { }
        public UserModel(ClaimsPrincipal identity)
        {
          
            Email = identity.FindFirst( "http://das/employer/identity/claims/email_address").Value;
            GivenName = identity.FindFirst("http://das/employer/identity/claims/given_name").Value;
            FamilyName = identity.FindFirst("http://das/employer/identity/claims/family_name").Value;
            DisplayName = identity.FindFirst("http://das/employer/identity/claims/display_name").Value;
            Id = Guid.Parse( identity.FindFirst("http://das/employer/identity/claims/id").Value);
        }
    }


}
