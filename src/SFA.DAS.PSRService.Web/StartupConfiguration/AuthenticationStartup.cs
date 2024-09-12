using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.GovUK.Auth.AppStart;
using SFA.DAS.GovUK.Auth.Authentication;
using SFA.DAS.GovUK.Auth.Configuration;
using SFA.DAS.GovUK.Auth.Services;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Configuration.Authorization;
using SFA.DAS.PSRService.Web.Services;
using SFA.DAS.PSRService.Web.Middleware.Authorization;
using SFA.DAS.PSRService.Web.Utils;

namespace SFA.DAS.PSRService.Web.StartupConfiguration
{
    public static class AuthenticationStartup
    {
        private static IWebConfiguration _configuration;

        public static void AddAuthorizationService(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    PolicyNames
                        .HasEmployerAccount
                    , policy =>
                    {
                        policy.RequireAuthenticatedUser();
                        policy.RequireClaim(EmployerPsrsClaims.AccountsClaimsTypeIdentifier);
                        policy.Requirements.Add(new EmployerAccountRequirement());
                        policy.Requirements.Add(new AccountActiveRequirement());
                    });
                options.AddPolicy(
                    PolicyNames
                        .CanEditReport
                    , policy =>
                    {
                        policy.Requirements.Add(new CanEditReport());
                        policy.Requirements.Add(new EmployerAccountRequirement());
                        policy.Requirements.Add(new AccountActiveRequirement());
                    });
                options.AddPolicy(
                    PolicyNames
                        .CanSubmitReport
                    , policy =>
                    {
                        policy.Requirements.Add(new CanSubmitReport());
                        policy.Requirements.Add(new EmployerAccountRequirement());
                        policy.Requirements.Add(new AccountActiveRequirement());
                    });
#if DEBUG
                options.AddPolicy(
                    "StubAuth",policy=>
                    {
                        policy.RequireAuthenticatedUser();
                    });
#endif
            });

            services.AddSingleton<IAuthorizationHandler, EmployerAccountHandler>();
            services.AddSingleton<IAuthorizationHandler, CanSubmitReportHandler>();
            services.AddSingleton<IAuthorizationHandler, CanEditReportHandler>();

            services.AddSingleton<IAuthorizationHandler, AccountActiveAuthorizationHandler>();//TODO remove after gov one login go live
            services.AddTransient<IStubAuthenticationService, StubAuthenticationService>();//TODO remove after gov one login go live
        }

        public static void AddAndConfigureAuthentication(this IServiceCollection services, IWebConfiguration configuration, IConfiguration config)
        {
            _configuration = configuration;
            
            services.AddTransient<ICustomClaims, EmployerAccountPostAuthenticationClaimsHandler>();
            
            services.Configure<GovUkOidcConfiguration>(config.GetSection("GovUkOidcConfiguration"));
            services.AddAndConfigureGovUkAuthentication(config, typeof(EmployerAccountPostAuthenticationClaimsHandler), "", "/SignIn-Stub");
        }
    }
}