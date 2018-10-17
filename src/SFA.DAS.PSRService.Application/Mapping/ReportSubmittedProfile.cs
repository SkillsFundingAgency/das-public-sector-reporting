using AutoMapper;
using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Messages.Events;

namespace SFA.DAS.PSRService.Application.Mapping
{
    public class ReportSubmittedProfile : Profile
    {
        public ReportSubmittedProfile()
        {
            CreateMap<Report, ReportSubmitted>()
                .ConvertUsing(report => new ReportSubmitted());
        }
    }
}
