using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
                .AfterMap((src, dest) =>
                {

                    dynamic dataObject = JObject.Parse(src.ReportingData);

                    dest.OrganisationName = dataObject.OrganisationName;


                });

            CreateMap<Report, ReportDto>()
                .ForMember(dest => dest.ReportingData, opts => opts.MapFrom(src => SerializeData(src)));

        }

        private string SerializeData(Report report)
        {
            var serilizationObj = new { OrganisationName = report.OrganisationName, Questions = "", Submitted = report.SubmittedDetails };

            return JsonConvert.SerializeObject(serilizationObj);
        }
    }
}
