using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using NLog.Web;

namespace SFA.DAS.PSRService.Web;

public class Program
{
    public static void Main(string[] args)
    {
        var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
        try
        {
            logger.Info("Starting up host");
            BuildWebHost(args).Run();
        }
        catch (Exception ex)
        {
            //NLog: catch setup errors
            logger.Error(ex, "Stopped program because of exception");
            throw;
        }
    }

    private static IWebHost BuildWebHost(string[] args)
    {
        return WebHost.CreateDefaultBuilder(args)
            .ConfigureServices(
                services =>
                {
                    _ = services
                        .Where(x => x.ServiceType == typeof(IWebHostEnvironment))
                        .Select(x => (IWebHostEnvironment) x.ImplementationInstance)
                        .First();
                })
            .UseStartup<Startup>()
            .UseNLog()
            .Build();
    }
}