using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.PSRService.Application.EmployerUserAccounts;
using SFA.DAS.PSRService.Application.Interfaces;
using SFA.DAS.PSRService.Data;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Services;

namespace SFA.DAS.PSRService.Web.StartupConfiguration;

public static class ApplicationServiceRegistrations
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IWebConfiguration webConfiguration)
    {
        services.AddTransient<IEmployerAccountService, EmployerAccountService>();
        services.AddTransient<IEmployerUserAccountsService, EmployerUserAccountsService>();
        services.AddTransient<IAccountApiClient, AccountApiClient>();
        services.AddTransient<IAccountApiConfiguration, AccountApiConfiguration>();
        services.AddSingleton<IAccountApiConfiguration>(webConfiguration.AccountsApi);

        services.AddScoped<IReportRepository, SqlReportRepository>();
        services.AddTransient<IReportService, ReportService>();
        services.AddTransient<IPeriodService, PeriodService>();
        services.AddTransient<IDateTimeService, SystemDateTimeService>();
        services.AddTransient<IUserService, UserService>();
        
        return services;
    }
}