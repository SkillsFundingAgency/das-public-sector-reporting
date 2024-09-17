using System.Security.Claims;
using SFA.DAS.PSRService.Web.Models;

namespace SFA.DAS.PSRService.Web.Services;

public interface IUserService
{
    UserModel GetUserModel(ClaimsPrincipal identity);
}