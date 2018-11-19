using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using NUnit.Framework;
using SFA.DAS.PSRService.Application.Mapping;

namespace SFA.DAS.PSRService.Application.UnitTests.MappingTests.Given_A_Report_To_ReportUpdated_Mapper
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public abstract class Given_A_Report_To_ReportUpdated_Mapper
    :GivenWhenThen<IMapper>
    {
        private MapperConfiguration _config;

        protected override void Given()
        {
            _config = new MapperConfiguration(cfg => cfg.AddProfile<ReportUpdatedProfile>());

            SUT = _config.CreateMapper();
        }

        [Test]
        public void Then_Mapping_Configuration_Is_Valid()
        {
            _config.AssertConfigurationIsValid();
        }
    }
}