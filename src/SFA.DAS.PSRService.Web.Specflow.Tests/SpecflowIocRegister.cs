using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using BoDi;
using SFA.DAS.PSRService.Web.Specflow.Tests.consts;
using TechTalk.SpecFlow;

namespace SFA.DAS.PSRService.Web.Specflow.Tests
{
    [Binding]
    public class SpecflowIocRegister
    {
        [BeforeTestRun]
        public static void RegisterTypes(IObjectContainer objectContainer)
        {
            //var connectionString = ConfigurationManager.ConnectionStrings[PersistenceNames.PsrsDBConnectionString]
            //    .ConnectionString;

            //var sqlConnection = new SqlConnection(connectionString);
            //objectContainer.RegisterInstanceAs<IDbConnection>(sqlConnection);
        }
    }
}