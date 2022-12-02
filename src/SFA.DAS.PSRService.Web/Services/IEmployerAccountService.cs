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
        EmployerIdentifier GetCurrentEmployerAccountId(HttpContext routeData);
        Task<Claim> GetClaim(string userId);
    }
}