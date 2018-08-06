using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.PSRService.Web.Models;
using Microsoft.AspNetCore.Routing;

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
