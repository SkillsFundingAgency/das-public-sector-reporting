using System.Security.Claims;
using System.Threading.Tasks;
using SFA.DAS.PSRService.Api.Types.Models;
using SFA.DAS.PSRService.Application.Api.Client.Clients;
using SFA.DAS.PSRService.Application.Api.Client.Exceptions;
using SFA.DAS.PSRService.Domain.Enums;
using SFA.DAS.PSRService.Settings;

namespace SFA.DAS.PSRService.Web.Orchestrators
{
    public class LoginOrchestrator : ILoginOrchestrator
    {
        //private readonly IWebConfiguration _config;
        //private readonly IContactsApiClient _contactsApiClient;

        //public LoginOrchestrator(IWebConfiguration config, IOrganisationsApiClient organisationsApiClient, IContactsApiClient contactsApiClient)
        //{
        //    _config = config;
        //    _organisationsApiClient = organisationsApiClient;
        //    _contactsApiClient = contactsApiClient;
        //}

        public async Task<LoginResult> Login(ClaimsPrincipal principal)
        {
            //var ukprn = principal.FindFirst("http://schemas.portal.com/ukprn")?.Value;
            //var username = principal.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/upn")?.Value;
            //var email = principal.FindFirst("http://schemas.portal.com/mail")?.Value;
            //var displayName = principal.FindFirst("http://schemas.portal.com/displayname")?.Value;

            //if (UserDoesNotHaveAcceptableRole(principal))
            //{
            //    return LoginResult.InvalidRole;
            //}

            //try
            //{
            //    organisation = await _organisationsApiClient.Get(ukprn);
            //}
            //catch (EntityNotFoundException)
            //{
            //    return LoginResult.NotRegistered;
            //}

            //try
            //{
            //    await GetContact(username, email, displayName);
            //}
            //catch (EntityNotFoundException)
            //{
            //    await CreateNewContact(email, organisation, displayName, username);
            //}
            return LoginResult.Valid;
        }

        //private bool UserDoesNotHaveAcceptableRole(ClaimsPrincipal principal)
        //{
        //    return !principal.HasClaim("http://schemas.portal.com/service", _config.Authentication.Role);
        //}

        //private async Task GetContact(string username, string email, string displayName)
        //{
        //    var contact = await _contactsApiClient.GetByUsername(username);

        //    await CheckStoredUserDetailsForUpdate(contact.Username, email, displayName, contact);
        //}

        //private async Task CheckStoredUserDetailsForUpdate(string userName, string email, string displayName, Report report)
        //{
        //    if (report.Email != email || report.DisplayName != displayName)
        //    {
        //        await _contactsApiClient.Update(new UpdateReportRequest()
        //        {
        //            Email = email,
        //            DisplayName = displayName,
        //            Username = userName
        //        });
        //    }
        //}

        //private async Task CreateNewContact(string email, Organisation organisation, string displayName,
        //    string username)
        //{
        //    var contact = await _contactsApiClient.Create(
        //        new CreateReportRequest()
        //        {
        //            Email = email,
        //            DisplayName = displayName,
        //            Username = username,
        //            EndPointAssessorOrganisationId = organisation.EndPointAssessorOrganisationId
        //        });

        //    await SetNewOrganisationPrimaryContact(organisation, contact);
        //}

        //private async Task SetNewOrganisationPrimaryContact(Organisation organisation, Report report)
        //{
        //    if (organisation.OrganisationStatus == OrganisationStatus.New)
        //    {
        //        await _organisationsApiClient.Update(new UpdateOrganisationRequest()
        //        {
        //            EndPointAssessorName = organisation.EndPointAssessorName,
        //            EndPointAssessorOrganisationId = organisation.EndPointAssessorOrganisationId,
        //            PrimaryContact = report.Username
        //        });
        //    }
        //}
    }
}