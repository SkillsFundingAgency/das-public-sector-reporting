using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.PSRService.Application.Api.Client;
using SFA.DAS.PSRService.Application.Api.Client.Clients;
using SFA.DAS.PSRService.Settings;
using SFA.DAS.PSRService.Web.Infrastructure;
using SFA.DAS.PSRService.Web.StartupConfiguration;
using StructureMap;
using SessionCache = SFA.DAS.PSRService.Application.Api.Client.SessionCache;

namespace SFA.DAS.PSRService.Web
{
    public class Startup
    {
        private const string ServiceName = "SFA.DAS.PSRService";
        private const string Version = "1.0";

        public Startup(IConfiguration config)
        {
            Configuration = ConfigurationService.GetConfig(config["Environment"], config["ConnectionStrings:Storage"], Version, ServiceName).Result;
        }

        public IWebConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddAndConfigureAuthentication(Configuration);
            services.AddMvc().AddControllersAsServices().AddSessionStateTempDataProvider();
            services.AddSession();

            return ConfigureIOC(services); 
        }

        private IServiceProvider ConfigureIOC(IServiceCollection services)
        {
            var container = new Container();

            container.Configure(config =>
            {
                config.Scan(_ =>
                {
                    _.AssemblyContainingType(typeof(Startup));
                    _.WithDefaultConventions();
                });

                config.For<IHttpClient>().Use<StandardHttpClient>();
                config.For<ICache>().Use<SessionCache>();
                config.For<ITokenService>().Use<TokenService>();
                config.For<IWebConfiguration>().Use(Configuration);
                config.For<IContactsApiClient>().Use<ContactsApiClient>().Ctor<string>().Is(Configuration.ClientApiAuthentication.ApiBaseAddress);

                config.Populate(services);
            });

            return container.GetInstance<IServiceProvider>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            
            app.UseStaticFiles()
                .UseSession()
                .UseAuthentication()
                .UseMvc(routes =>
                {
                    routes.MapRoute(
                        name: "default",
                        template: "{controller=Home}/{action=Index}/{id?}");
                });
        }
    }
}