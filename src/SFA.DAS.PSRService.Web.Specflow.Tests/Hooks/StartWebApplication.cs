using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using TechTalk.SpecFlow;
using SFA.DAS.PSRService.Web;
using SFA.DAS.PSRService.Web.Specflow.Tests.TestSupport;

namespace SFA.DAS.PSRService.Web.Specflow.Tests.Framework
{
    [Binding]
    public sealed class StartWebApplication
    {
        // For additional details on SpecFlow hooks see http://go.specflow.org/doc-hooks
        private static Process _webprocess;
        [BeforeTestRun]
        public static void StartApplication()
        {

            if(Configurator.GetConfiguratorInstance().GetBaseUrl().Contains("localhost"))
            { 
            _webprocess = new Process
            {
                StartInfo =
                {
                    FileName = "dotnet",
                    Arguments = "run",
                    UseShellExecute = false,
                    WorkingDirectory = @"C:\Source\das-public-sector-reporting\src\SFA.DAS.PSRService.Web\"
                }
            };
            _webprocess.Start();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Configurator.GetConfiguratorInstance().GetBaseUrl() + Configurator.GetConfiguratorInstance().GetEmployerId() + "/home/");

                    bool webAppRunning = false;
                    int sleepTimerSec = 2;
                    int totalStartupTime = 0;


                    while (webAppRunning == false)
                    {
                        if (totalStartupTime > 90)
                        {
                            StopApplication();

                            throw new TimeoutException(
                                "Startup of WebApp took longer than 90 seconds, Aborting test run");
                        }

                        Thread.Sleep(sleepTimerSec * 1000);

                        totalStartupTime = +sleepTimerSec;

                        try
                        {
                            var result = client.GetAsync("").Result;
                            webAppRunning = result.IsSuccessStatusCode;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    }
                }
            }
        }

        [AfterTestRun]
        public static void StopApplication()
        {
            if (_webprocess != null && _webprocess.HasExited == false)
            {
                _webprocess.CloseMainWindow();
                _webprocess.Close();
            }
        }
    }
}
