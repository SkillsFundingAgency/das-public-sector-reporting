using AutoMapper;
using Newtonsoft.Json;
using SFA.DAS.PSRService.Application.Domain;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Application.Mapping;

public class AuditRecordMappingProfile : Profile
{
    public AuditRecordMappingProfile()
    {
        CreateMap<AuditRecordDto, AuditRecord>()
            .ForMember(dest => dest.Sections, opts => opts.Ignore())
            .ForMember(dest => dest.UpdatedBy, opts => opts.MapFrom(s => s == null ? null : JsonConvert.DeserializeObject<User>(s.UpdatedBy)))
            .AfterMap((src, dest) =>
            {
                var dataObject = JsonConvert.DeserializeObject<ReportingData>(src.ReportingData);

                dest.Sections = dataObject.Questions;
                dest.ReportingPercentages = dataObject.ReportingPercentages;
                dest.ReportingPercentagesSchools = dataObject.ReportingPercentagesSchools;
                dest.OrganisationName = dataObject.OrganisationName;
                dest.SerialNo = dataObject.SerialNo;
                dest.HasMinimumEmployeeHeadcount = dataObject.HasMinimumEmployeeHeadcount;
                dest.IsLocalAuthority = dataObject.IsLocalAuthority;
            });
    }
}