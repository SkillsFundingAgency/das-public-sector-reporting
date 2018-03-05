using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.PSRService.Settings;
using SFA.DAS.PSRService.Application.Api.Extensions;

namespace SFA.DAS.PSRService.Application.Api.StartupConfiguration
{
    public static class AuthenticationStartup
    {
        public static void AddAndConfigureAuthentication(this IServiceCollection services,
            IWebConfiguration configuration)
        {

            

            //services.AddAuthentication(sharedOptions =>
            //    {
            //        sharedOptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            //    })
            //    .AddJwtBearer(options =>
            //    {
            //        options.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            ValidateIssuer = true,
            //            ValidateAudience = true,
            //            ValidateLifetime = true,
            //            ValidateIssuerSigningKey = true,
            //            ValidIssuer = "SFA.DAS.PSRService",
            //            ValidAudience = "SFA.DAS.PSRService.api",
            //            IssuerSigningKey = new SymmetricSecurityKey(
            //                Encoding.UTF8.GetBytes(configuration.Api.TokenEncodingKey))
            //        };
            //    });
        }
    }
}