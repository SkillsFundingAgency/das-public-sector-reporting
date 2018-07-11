using AutoMapper;
using Newtonsoft.Json;
using SFA.DAS.PSRService.Application.Domain;
using SFA.DAS.PSRService.Domain.Entities;
using System;

namespace SFA.DAS.PSRService.Application.Mapping
{
    public class AuditRecordMappingProfile : Profile
    {
        public AuditRecordMappingProfile()
        {
            CreateMap<AuditRecordDto, AuditRecord>()
                .ForMember(dest => dest.OrganisationName, opts => opts.Ignore())
                .ForMember(dest => dest.ReportingPercentages, opts => opts.Ignore())
                .ForMember(dest => dest.Sections, opts => opts.Ignore())
                .ForMember(dest => dest.UpdatedBy, opts => opts.MapFrom(s => s == null ? null : JsonConvert.DeserializeObject<User>(s.UpdatedBy)))
                .ForMember(dest => dest.UpdatedUtc, 
                           opts => opts.MapFrom(
                               src => DateTime.SpecifyKind(src.UpdatedUtc, DateTimeKind.Utc)))
                .AfterMap((src, dest) =>
                {
                    var dataObject = JsonConvert.DeserializeObject<ReportingData>(src.ReportingData);

                    dest.Sections = dataObject.Questions;
                    dest.ReportingPercentages = dataObject.ReportingPercentages;
                    dest.OrganisationName = dataObject.OrganisationName;
                });
        }
    }
}