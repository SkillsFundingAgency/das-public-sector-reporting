using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SFA.DAS.PSRService.Web.Models;

namespace SFA.DAS.PSRService.Web.Services
{
    public interface IEmployerAccountService
    {
        EmployerIdentifier GetCurrentEmployerAccountId(HttpContext routeData);
        Task<Claim> GetClaim(string userId);
    }
}