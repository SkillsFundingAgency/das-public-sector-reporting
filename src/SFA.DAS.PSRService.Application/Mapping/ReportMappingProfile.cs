using System.Runtime.InteropServices.ComTypes;
using AutoMapper;
using Newtonsoft.Json;
using SFA.DAS.PSRService.Application.Domain;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Application.Mapping
{
    public class ReportMappingProfile : Profile
    {
        public ReportMappingProfile()
        {
            CreateMap<ReportDto, Report>()
                .ForMember(dest => dest.SubmittedDetails, opts => opts.Ignore())
                .ForMember(dest => dest.OrganisationName, opts => opts.Ignore())
                .ForMember(dest => dest.Sections, opts => opts.Ignore())
                .ForMember(dest => dest.ReportingPercentages, opts => opts.Ignore())
                .ForMember(dest => dest.Period, opts => opts.MapFrom(s => new Period(s.ReportingPeriod)))
                .ForMember(dest => dest.UpdatedBy, opts => opts.MapFrom(s => s == null ? null : JsonConvert.DeserializeObject<User>(s.UpdatedBy)))
                .AfterMap((src, dest) =>
                {
                    var dataObject = JsonConvert.DeserializeObject<ReportingData>(src.ReportingData);

                    dest.OrganisationName = dataObject.OrganisationName;
                    dest.Sections = dataObject.Questions;
                    dest.ReportingPercentages = dataObject.ReportingPercentages;
                    dest.SubmittedDetails = dataObject.Submitted;
                });

            CreateMap<Report, ReportDto>()
                .ForMember(dest => dest.ReportingPeriod, opts => opts.MapFrom(src => new Period(src.ReportingPeriod).PeriodString))
                .ForMember(dest => dest.ReportingData, opts => opts.MapFrom(src => SerializeData(src)))
                .ForMember(dest => dest.UpdatedBy, opts => opts.MapFrom(src => JsonConvert.SerializeObject(src.UpdatedBy)));
        }

        private string SerializeData(Report report)
        {
            var serilizationObj = new
            {
                report.OrganisationName, 
                Questions = report.Sections, 
                Submitted = report.SubmittedDetails,
                report.ReportingPercentages
            };

            return JsonConvert.SerializeObject(serilizationObj);
        }
    }
}

