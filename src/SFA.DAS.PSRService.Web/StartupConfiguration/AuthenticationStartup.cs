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

        if (configuration.UseGovSignIn)
        {
            services.Configure<GovUkOidcConfiguration>(config.GetSection("GovUkOidcConfiguration"));
            services.AddAndConfigureGovUkAuthentication(config, typeof(EmployerAccountPostAuthenticationClaimsHandler), "", "/SignIn-Stub");
        }
        else
        {
            services.AddAuthentication(sharedOptions =>
                {
                    sharedOptions.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    sharedOptions.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                    sharedOptions.DefaultSignOutScheme = OpenIdConnectDefaults.AuthenticationScheme;
                })
                .AddOpenIdConnect(options =>
                {
                    options.ClientId = configuration.Identity.ClientId;
                    options.ClientSecret = configuration.Identity.ClientSecret;
                    options.Authority = configuration.Identity.Authority;
                    options.ResponseType = "code";
                    options.UsePkce = false;

                    var scopes = GetScopes(configuration);
                    foreach (var scope in scopes)
                    {
                        options.Scope.Add(scope);
                    }

                    var mapUniqueJsonKeys = GetMapUniqueJsonKey(configuration);
                    options.ClaimActions.MapUniqueJsonKey(mapUniqueJsonKeys[0], mapUniqueJsonKeys[1]);

                    options.Events.OnRemoteFailure = c =>
                    {
                        if (c.Failure.Message.Contains("Correlation failed"))
                        {
                            c.Response.Redirect("/");
                            c.HandleResponse();
                        }

                        return Task.CompletedTask;
                    };
                })
                .AddCookie(options =>
                {
                    options.AccessDeniedPath = new PathString("/Service/AccessDenied");
                    options.Events.OnRedirectToAccessDenied = RedirectToAccessDenied;
                    options.ExpireTimeSpan = TimeSpan.FromHours(1);
                    options.Cookie.Name = "SFA.DAS.PRSService.Web.Auth";
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.SlidingExpiration = true;
                    options.Cookie.SameSite = SameSiteMode.None;
                    options.CookieManager = new ChunkingCookieManager { ChunkSize = 3000 };
                });
            services
                .AddOptions<OpenIdConnectOptions>(OpenIdConnectDefaults.AuthenticationScheme)
                .Configure<IEmployerAccountService>((options, accountsService) => { options.Events.OnTokenValidated = async (ctx) => { await PopulateAccountsClaim(ctx, accountsService); }; });
        }
    }

    private static Task RedirectToAccessDenied(RedirectContext<CookieAuthenticationOptions> context)
    {
        var routeData = context.HttpContext.GetRouteData();
        var path = context.Request.Path.Value;
        path = path.EndsWith('/') ? path.Substring(0, path.Length - 1) : path;

        if (path.Contains("Home/Index") || path.Equals($"/Accounts/{routeData.Values["hashedEmployerAccountId"]}"))
        {
            //default path
            context.Response.Redirect(context.RedirectUri);
        }
        else
        {
            //custom
            context.Response.Redirect(context.Request.PathBase + $"/Accounts/{routeData.Values["hashedEmployerAccountId"]}/Home/Index");
        }

        return Task.CompletedTask;
    }

    private static List<string> GetScopes(IWebConfiguration configuration)
    {
        return configuration.Identity.Scopes.Split(' ').ToList();
    }

    private static List<string> GetMapUniqueJsonKey(IWebConfiguration configuration)
    {
        return configuration.Identity.MapUniqueJsonKey.Split(' ').ToList();
    }

    private static async Task PopulateAccountsClaim(TokenValidatedContext ctx, IEmployerAccountService accountsSvc)
    {
        var userId = ctx.Principal.Claims.First(c => c.Type.Equals(EmployerPsrsClaims.IdamsUserIdClaimTypeIdentifier)).Value;
        var associatedAccountsClaim = await accountsSvc.GetClaim(userId);
        ctx.Principal.Identities.First().AddClaim(associatedAccountsClaim);
    }
}