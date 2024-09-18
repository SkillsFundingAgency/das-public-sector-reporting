using System.Security.Claims;
using Microsoft.Extensions.Logging;
using SFA.DAS.PSRService.Web.Models;

namespace SFA.DAS.PSRService.Web.Services;

public interface IUserService
{
    UserModel GetUserModel(ClaimsPrincipal identity);
}

public class UserService(ILogger<UserService> logger) : IUserService
{
    public UserModel GetUserModel(ClaimsPrincipal identity)
    {
        try
        {
            return new UserModel(identity);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Unable to map claims for user {IdentityName}", identity.Identity?.Name);
            throw;
        }
    }
}