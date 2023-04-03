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
                .ForMember(dest => dest.HasMinimumEmployeeHeadcount, opts => opts.Ignore())
                .ForMember(dest => dest.IsLocalAuthority, opts => opts.Ignore())
                .ForMember(dest => dest.Sections, opts => opts.Ignore())
                .ForMember(dest => dest.SerialNo, opts => opts.Ignore())
                .ForMember(dest => dest.ReportingPercentages, opts => opts.Ignore())
                .ForMember(dest => dest.ReportingPercentagesSchools, opts => opts.Ignore())
                .ForMember(dest => dest.Period, opts => opts.MapFrom(s => Period.ParsePeriodString(s.ReportingPeriod)))
                .ForMember(dest => dest.UpdatedBy, opts => opts.MapFrom(s => s == null ? null : JsonConvert.DeserializeObject<User>(s.UpdatedBy)))
                .AfterMap((src, dest) =>
                {
                    var dataObject = JsonConvert.DeserializeObject<ReportingData>(src.ReportingData);

                    dest.OrganisationName = dataObject.OrganisationName;
                    dest.HasMinimumEmployeeHeadcount = dataObject.HasMinimumEmployeeHeadcount;
                    dest.IsLocalAuthority = dataObject.IsLocalAuthority;
                    dest.Sections = dataObject.Questions;
                    dest.SerialNo = dataObject.SerialNo;
                    dest.ReportingPercentages = dataObject.ReportingPercentages;
                    dest.ReportingPercentagesSchools = dataObject.ReportingPercentagesSchools;
                    dest.SubmittedDetails = dataObject.Submitted;
                });

            CreateMap<Report, ReportDto>()
                .ForMember(dest => dest.ReportingPeriod, opts => opts.MapFrom(src => Period.ParsePeriodString(src.ReportingPeriod).PeriodString))
                .ForMember(dest => dest.ReportingData, opts => opts.MapFrom(src => SerializeData(src)))
                .ForMember(dest => dest.UpdatedBy, opts => opts.MapFrom(src => JsonConvert.SerializeObject(src.UpdatedBy)));
        }

        private string SerializeData(Report report)
        {
            var serilizationObj = new
            {
                report.OrganisationName,
                report.HasMinimumEmployeeHeadcount,
                report.IsLocalAuthority,
                Questions = report.Sections,
                SerialNo = report.SerialNo,
                Submitted = report.SubmittedDetails,
                report.ReportingPercentages,
                report.ReportingPercentagesSchools
            };

            return JsonConvert.SerializeObject(serilizationObj);
        }
    }
}

