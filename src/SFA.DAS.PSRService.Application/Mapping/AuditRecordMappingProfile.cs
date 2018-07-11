using AutoMapper;
using Newtonsoft.Json;
using SFA.DAS.PSRService.Application.Domain;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Application.Mapping
{
    public class AuditRecordMappingProfile : Profile
    {
        public AuditRecordMappingProfile()
        {
            CreateMap<AuditRecordDto, AuditRecord>()
                .ConvertUsing(src =>
                {
                    var updatedBy = src.UpdatedBy == null ? null : JsonConvert.DeserializeObject<User>(src.UpdatedBy);
                    var dataObject = JsonConvert.DeserializeObject<ReportingData>(src.ReportingData);

                    return new AuditRecord(dataObject.OrganisationName, dataObject.Questions, dataObject.ReportingPercentages, updatedBy, src.UpdatedUtc);
                });
        }
    }
}