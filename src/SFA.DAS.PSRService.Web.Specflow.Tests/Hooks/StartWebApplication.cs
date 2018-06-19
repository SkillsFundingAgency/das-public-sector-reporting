using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
                    WorkingDirectory = BuildWorkingDirectory()
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

        private static string BuildWorkingDirectory()
        {
            var thisType = typeof(StartWebApplication);

            var assemblyPath =
                Path
                    .GetDirectoryName(
                        thisType
                            .Assembly
                            .Location);


            var sourcePath =
                new DirectoryInfo(assemblyPath)
                    .Parent
                    .Parent
                    .Parent
                    .FullName;

            return
                Path
                    .Combine(
                        sourcePath,
                        @"SFA.DAS.PSRService.Web");
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
