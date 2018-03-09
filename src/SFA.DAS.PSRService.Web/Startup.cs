using System;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.PSRService.Application.Interfaces;
using SFA.DAS.PSRService.Application.ReportHandlers;
using SFA.DAS.PSRService.Data;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Services;
using SFA.DAS.PSRService.Web.StartupConfiguration;
using StructureMap;

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

                //config.For<IHttpClient>().Use<StandardHttpClient>();
                //config.For<ICache>().Use<SessionCache>();
                //config.For<ITokenService>().Use<TokenService>();
                config.For<IWebConfiguration>().Use(Configuration);
                //config.For<IContactsApiClient>().Use<ContactsApiClient>().Ctor<string>().Is(Configuration.ClientApiAuthentication.ApiBaseAddress);
                config.For<IReportService>().Use<ReportService>();
                config.For<IReportRepository>().Use<ReportRepository>().Ctor<string>().Is(Configuration.SqlConnectionString);

                config.Populate(services);


                config.Scan(scanner =>
                {
                    scanner.AssemblyContainingType<GetReportRequest>(); // Our assembly with requests & handlers
                    scanner.ConnectImplementationsToTypesClosing(typeof(IRequestHandler<>)); // Handlers with no response
                    scanner.ConnectImplementationsToTypesClosing(typeof(IRequestHandler<,>)); // Handlers with a response
                    scanner.ConnectImplementationsToTypesClosing(typeof(INotificationHandler<>));
                });
                config.For<SingleInstanceFactory>().Use<SingleInstanceFactory>(ctx => t => ctx.GetInstance(t));
                config.For<MultiInstanceFactory>().Use<MultiInstanceFactory>(ctx => t => ctx.GetAllInstances(t));
                config.For<IMediator>().Use<Mediator>();
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