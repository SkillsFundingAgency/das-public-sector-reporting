using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.WsFederation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using SFA.DAS.PSRService.Web.Configuration;

namespace SFA.DAS.PSRService.Web.StartupConfiguration
{
    public static class AuthenticationStartup
    {
        private static IWebConfiguration _configuration;

        public static void AddAndConfigureAuthentication(this IServiceCollection services, IWebConfiguration configuration)
        {
            _configuration = configuration;

            services.AddAuthentication(sharedOptions =>
                {
                    sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    sharedOptions.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                    // sharedOptions.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

                })
                .AddOpenIdConnect(options =>
                {
                    options.ClientId = "psrsdev";
                    options.ClientSecret = "psrs-secret";
                    options.AuthenticationMethod = OpenIdConnectRedirectBehavior.RedirectGet;
                    options.Authority = "https://test-login.apprenticeships.sfa.bis.gov.uk/identity";
                    options.ResponseType = "code";
                   // options.GetClaimsFromUserInfoEndpoint = true;
                    options.SaveTokens = true;
                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                    options.ClaimActions.MapUniqueJsonKey("id", "sub");

                    options.ClaimActions.MapUniqueJsonKey("email", "name");

                    //options.TokenValidationParameters = new TokenValidationParameters()
                    //{
                    //    NameClaimType = "http://das/employer/identity/claims/email_address"
                    //};

                })
                .AddCookie();
            //.AddCookie(options => { options.ReturnUrlParameter = "/Account/SignedIn"; });
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
    }
}