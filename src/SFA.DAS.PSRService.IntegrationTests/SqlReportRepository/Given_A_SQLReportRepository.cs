using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using Moq;
using NUnit.Framework;
using SFA.DAS.PSRService.Application.Interfaces;
using SFA.DAS.PSRService.Data;
using SFA.DAS.UnitOfWork;

namespace SFA.DAS.PSRService.IntegrationTests.SqlReportRepository
{
    [ExcludeFromCodeCoverage]
    public class Given_A_SQLReportRepository : GivenWhenThen<IReportRepository>
    {
        private SqlConnection _connection;
        private SqlTransaction _trans;

        protected override void Given()
        {
            RepositoryTestHelper
                .ClearData();

            _connection = new SqlConnection(RepositoryTestHelper.ConnectionString);

            _connection.Open();

            _trans = _connection.BeginTransaction();


            SUT = new SQLReportRepository(() => _connection, () => _trans);
        }

        protected override void When()
        {
            _trans.Commit();
        }

        [TearDown]
        public void ClearDatabase()
        {
            _connection.Close();

            RepositoryTestHelper
                .ClearData();
        }
    }
}