using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SFA.DAS.PSRService.Web.Models;

namespace SFA.DAS.PSRService.Web.Services
{
    public interface IEmployerAccountService
    {
        Task<IEnumerable<EmployerIdentifier>> GetEmployerIdentifiersAsync(string userId);
        EmployerIdentifier GetCurrentEmployerAccountId(HttpContext routeData);
        Task<IEnumerable<EmployerIdentifier>> GetUserRoles(IList<EmployerIdentifier> values, string userId);
        Task<Claim> GetClaim(string userId);
    }
}