using System;
using System.Collections.Generic;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.WsFederation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Services;
using SFA.DAS.PSRService.Web.Middleware;

namespace SFA.DAS.PSRService.Web.StartupConfiguration
{
    public static class AuthenticationStartup
    {
        private static IWebConfiguration _configuration;
        private const string HasEmployerAccountPolicyName = "HasEmployerAccount";

        public static void AddAuthorizationService(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(HasEmployerAccountPolicyName, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim(EmployerPsrsClaims.AccountsClaimsTypeIdentifier);
                    policy.Requirements.Add(new EmployerAccountRequirement());
                });
            });

            services.AddSingleton<IAuthorizationHandler, EmployerAccountHandler>();
        }

        public static void AddAndConfigureAuthentication(this IServiceCollection services, IWebConfiguration configuration, IEmployerAccountService accountsSvc)
        {
            _configuration = configuration;

            services.AddAuthentication(sharedOptions =>
                {
                    sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    sharedOptions.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                    sharedOptions.DefaultSignOutScheme = OpenIdConnectDefaults.AuthenticationScheme;
                })
                .AddOpenIdConnect(options =>
                {
                    options.ClientId = _configuration.Identity.ClientId;
                    options.ClientSecret = _configuration.Identity.ClientSecret;
                    options.AuthenticationMethod = (OpenIdConnectRedirectBehavior) _configuration.Identity.AuthenticationMethod;
                    options.Authority = _configuration.Identity.Authority;
                    options.ResponseType = _configuration.Identity.ResponseType;
                    options.SaveTokens = _configuration.Identity.SaveTokens;

                    var scopes = GetScopes();
                    foreach (var scope in scopes)
                    {
                        options.Scope.Add(scope);
                    }

                    var mapUniqueJsonKeys = GetMapUniqueJsonKey();
                    options.ClaimActions.MapUniqueJsonKey(mapUniqueJsonKeys[0], mapUniqueJsonKeys[1]);
                    options.Events.OnTokenValidated = async (ctx) => await PopulateAccountsClaim(ctx, accountsSvc);
                })
                .AddCookie(options => options.ExpireTimeSpan = TimeSpan.FromHours(1));
        }

        private static IEnumerable<string> GetScopes()
        {
            return _configuration.Identity.Scopes.Split(' ').ToList();
        }

        private static List<string> GetMapUniqueJsonKey()
        {
            return _configuration.Identity.MapUniqueJsonKey.Split(' ').ToList();
        }

        private static Task OnTokenValidated(SecurityTokenValidatedContext context)
        {
            //var ukprn = (context.Principal.FindFirst("http://schemas.portal.com/ukprn"))?.Value;

            //var jwt = new JwtBuilder().WithAlgorithm(new HMACSHA256Algorithm())
            //    .WithSecret(_configuration.Api.TokenEncodingKey)
            //    .Issuer("SFA.DAS.PSRService")
            //    .Audience("SFA.DAS.PSRService.api")
            //    .ExpirationTime(DateTime.Now.AddMinutes(5))
            //    .AddClaim("ukprn", ukprn)
            //    .Build();

            //context.HttpContext.Session.SetString(ukprn, jwt);

            return Task.FromResult(0);
        }

        private static async Task PopulateAccountsClaim(TokenValidatedContext ctx, IEmployerAccountService accountsSvc)
        {
            var userId = ctx.Principal.Claims.First(c => c.Type.Equals(EmployerPsrsClaims.IdamsUserIdClaimTypeIdentifier)).Value;
            var accounts = await accountsSvc.GetEmployerIdentifiersAsync(userId);
            var accountsAsJson = JsonConvert.SerializeObject(accounts);
            var associatedAccountsClaim = new Claim(EmployerPsrsClaims.AccountsClaimsTypeIdentifier, accountsAsJson, JsonClaimValueTypes.Json);

            ctx.Principal.Identities.First().AddClaim(associatedAccountsClaim);
        }
    }
}