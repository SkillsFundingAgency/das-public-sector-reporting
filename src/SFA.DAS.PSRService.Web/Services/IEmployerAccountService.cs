﻿using System.Collections.Generic;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SFA.DAS.PSRService.Web.Models;

namespace SFA.DAS.PSRService.Web.Services
{
    public interface IEmployerAccountService
    {
        Task<IDictionary<string, EmployerIdentifier>> GetEmployerIdentifiersAsync(string userId);

        EmployerIdentifier GetCurrentEmployerAccountId(HttpContext routeData);

    }
}
