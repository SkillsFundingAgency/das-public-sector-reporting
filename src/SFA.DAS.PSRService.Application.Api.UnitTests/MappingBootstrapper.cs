using SFA.DAS.AssessmentOrgs.Api.Client.Core.Types;
using SFA.DAS.PSRService.Api.Types.Models;
using SFA.DAS.PSRService.Application.Domain;

namespace SFA.DAS.PSRService.Application.Api.UnitTests
{
    class MappingBootstrapper
    {
        public static void Initialize()
        {
            SetupAutomapper();
        }

        public static void SetupAutomapper()
        {
            AutoMapper.Mapper.Reset();

            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<CreateReportRequest, ReportCreateDomainModel>();
                cfg.CreateMap<ReportCreateDomainModel, Report>();
            });
        }
    }
}
