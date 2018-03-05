using Moq;
using NUnit.Framework;
using SFA.DAS.PSRService.Application.Api.Client.Clients;
using SFA.DAS.PSRService.Settings;

namespace SFA.DAS.PSRService.Web.UnitTests.OrchestratorTests.LoginOrchestrator
{
    public class LoginOrchestratorTestBase
    {
        protected Orchestrators.LoginOrchestrator LoginOrchestrator;

        [SetUp]
        protected void Setup()
        {
            var config = new WebConfiguration() { Authentication = new AuthSettings() { Role = "EPA" } };

            LoginOrchestrator = new Orchestrators.LoginOrchestrator();
        }
    }
}