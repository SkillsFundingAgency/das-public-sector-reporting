using System.Security.Claims;
using System.Threading.Tasks;

namespace SFA.DAS.PSRService.Web.Orchestrators
{
    public interface ILoginOrchestrator
    {
        Task<LoginResult> Login(ClaimsPrincipal principal);
    }
}