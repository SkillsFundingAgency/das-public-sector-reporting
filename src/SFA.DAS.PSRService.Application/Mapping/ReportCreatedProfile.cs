using AutoMapper;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Messages.Events;

namespace SFA.DAS.PSRService.Application.Mapping
{
    public class ReportCreatedProfile : Profile
    {
        public ReportCreatedProfile()
        {
            CreateMap<Report, ReportCreated>()
                .ConvertUsing(src =>
                    {
                        return new ReportCreated
                        {
                            Id = src.Id,
                            EmployerId = src.EmployerId,
                            ReportingPeriod = src.ReportingPeriod,
                            Created = src.UpdatedUtc.Value
                        };
                    }
                );
        }
    }
}