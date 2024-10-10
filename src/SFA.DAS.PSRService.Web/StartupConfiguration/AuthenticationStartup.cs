using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.GovUK.Auth.AppStart;
using SFA.DAS.GovUK.Auth.Authentication;
using SFA.DAS.GovUK.Auth.Configuration;
using SFA.DAS.GovUK.Auth.Services;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Configuration.Authorization;
using SFA.DAS.PSRService.Web.Middleware.Authorization;

namespace SFA.DAS.PSRService.Web.StartupConfiguration;

public static class AuthenticationStartup
{
    public static void AddAuthorizationService(this IServiceCollection services)
    {
        services.AddAuthorizationBuilder()
            .AddPolicy(PolicyNames.HasEmployerAccount, policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim(EmployerPsrsClaims.AccountsClaimsTypeIdentifier);
                policy.Requirements.Add(new EmployerAccountRequirement());
                policy.Requirements.Add(new AccountActiveRequirement());
            })
            .AddPolicy(PolicyNames.CanEditReport, policy =>
            {
                policy.Requirements.Add(new CanEditReport());
                policy.Requirements.Add(new EmployerAccountRequirement());
                policy.Requirements.Add(new AccountActiveRequirement());
            })
            .AddPolicy(PolicyNames.CanSubmitReport, policy =>
            {
                policy.Requirements.Add(new CanSubmitReport());
                policy.Requirements.Add(new EmployerAccountRequirement());
                policy.Requirements.Add(new AccountActiveRequirement());
            })
            .AddPolicy("StubAuth", policy => { policy.RequireAuthenticatedUser(); });

        services.AddSingleton<IAuthorizationHandler, EmployerAccountHandler>();
        services.AddSingleton<IAuthorizationHandler, CanSubmitReportHandler>();
        services.AddSingleton<IAuthorizationHandler, CanEditReportHandler>();
        services.AddTransient<IStubAuthenticationService, StubAuthenticationService>();
    }

    public static void AddAndConfigureAuthentication(this IServiceCollection services, IWebConfiguration configuration, IConfiguration config)
    {
        services.AddTransient<ICustomClaims, EmployerAccountPostAuthenticationClaimsHandler>();

        services.Configure<GovUkOidcConfiguration>(config.GetSection("GovUkOidcConfiguration"));
        services.AddAndConfigureGovUkAuthentication(config, typeof(EmployerAccountPostAuthenticationClaimsHandler), "", "/SignIn-Stub");
    }
}
