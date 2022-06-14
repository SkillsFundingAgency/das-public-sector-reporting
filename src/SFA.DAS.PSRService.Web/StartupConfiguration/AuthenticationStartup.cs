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
using Microsoft.Extensions.DependencyInjection;
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
                    });
                options.AddPolicy(
                    PolicyNames
                        .CanEditReport
                    , policy => { policy.Requirements.Add(new CanEditReport()); });
                options.AddPolicy(
                    PolicyNames
                        .CanSubmitReport
                    , policy => { policy.Requirements.Add(new CanSubmitReport()); });
            });

            services.AddSingleton<IAuthorizationHandler, EmployerAccountHandler>();
            services.AddSingleton<IAuthorizationHandler, CanSubmitReportHandler>();
            services.AddSingleton<IAuthorizationHandler, CanEditReportHandler>();
        }

        public static void AddAndConfigureAuthentication(this IServiceCollection services, IWebConfiguration configuration, IEmployerAccountService accountsSvc)
        {
            _configuration = configuration;

            services.AddAuthentication(sharedOptions =>
                {
                    sharedOptions.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    sharedOptions.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                    sharedOptions.DefaultSignOutScheme = OpenIdConnectDefaults.AuthenticationScheme;

                })
                .AddOpenIdConnect(options =>
                {
                    options.MetadataAddress = $"{_configuration.Identity.Authority}.well-known/openid-configuration";
                    options.ClientId = _configuration.Identity.ClientId;
                    options.ClientSecret = _configuration.Identity.ClientSecret;
                    options.Authority = _configuration.Identity.Authority;
                    options.ResponseType = "code";
                    options.UsePkce = false;

                    var scopes = GetScopes();
                    foreach (var scope in scopes)
                    {
                        options.Scope.Add(scope);
                    }

                    var mapUniqueJsonKeys = GetMapUniqueJsonKey();
                    options.ClaimActions.MapUniqueJsonKey(mapUniqueJsonKeys[0], mapUniqueJsonKeys[1]);
                    options.Events.OnTokenValidated = async (ctx) => await PopulateAccountsClaim(ctx, accountsSvc);

                })
                .AddCookie(options =>
                {
                    options.AccessDeniedPath = new PathString("/Service/AccessDenied");
                    options.ExpireTimeSpan = TimeSpan.FromHours(1);
                    switch (configuration.SessionStore.Type)
                    {
                        case "Redis":
                            options.SessionStore = new RedisCacheTicketStore(configuration.SessionStore.Connectionstring);
                            break;
                        case "Default":
                            break;
                    }
                 
                    options.Events.OnRedirectToAccessDenied = RedirectToAccessDenied;

                });
        }

        private static Task RedirectToAccessDenied(RedirectContext<CookieAuthenticationOptions> context)
        {
            var routeData = context.HttpContext.GetRouteData();
            var path = context.Request.Path.Value;
             path = path.EndsWith("/") ? path.Substring(0, path.Length - 1) : path;

            if (path.Contains("Home/Index") || path.Equals($"/Accounts/{routeData.Values["employerAccountId"]}"))
            {
                //default path
                context.Response.Redirect(context.RedirectUri);
            }
            else
            { 
                //custom
                context.Response.Redirect(context.Request.PathBase + $"/Accounts/{routeData.Values["employerAccountId"]}/Home/Index");
            }

            return Task.CompletedTask;
        }

        private static IEnumerable<string> GetScopes()
        {
            return _configuration.Identity.Scopes.Split(' ').ToList();
        }

        private static List<string> GetMapUniqueJsonKey()
        {
            return _configuration.Identity.MapUniqueJsonKey.Split(' ').ToList();
        }

        private static async Task PopulateAccountsClaim(TokenValidatedContext ctx, IEmployerAccountService accountsSvc)
        {
            var userId = ctx.Principal.Claims.First(c => c.Type.Equals(EmployerPsrsClaims.IdamsUserIdClaimTypeIdentifier)).Value;
            var associatedAccountsClaim = await accountsSvc.GetClaim(userId);
            ctx.Principal.Identities.First().AddClaim(associatedAccountsClaim);
        }

    }
}