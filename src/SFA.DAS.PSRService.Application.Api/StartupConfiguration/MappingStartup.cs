using SFA.DAS.AssessmentOrgs.Api.Client.Core.Types;
using SFA.DAS.PSRService.Api.Types.Models;
using SFA.DAS.PSRService.Application.Domain;

namespace SFA.DAS.PSRService.Application.Api.StartupConfiguration
{
    public static class MappingStartup
    {
        public static void AddMappings()
        {
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<CreateReportRequest, ReportCreateDomainModel>();
                cfg.CreateMap<ReportCreateDomainModel, Report>();
            });
        }
    }
}