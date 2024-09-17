using System;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using SFA.DAS.PSRService.Web.Models;

namespace SFA.DAS.PSRService.Web.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;

        public UserService(ILogger<UserService> logger)
        {
            _logger = logger;
       
        }

        public UserModel GetUserModel(ClaimsPrincipal identity)
        {
            try
            {
                return new UserModel(identity);
            }
            catch (Exception e)
            {
                _logger.LogError(e,$"Unable to map claims for user {identity.Identity.Name}",identity);
                throw;
            }
        }
    }
}
