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
                .AfterMap((src, dest) =>
                {

                    dynamic dataObject = JObject.Parse(src.ReportingData);

                    dest.OrganisationName = dataObject.OrganisationName;


                });
            
            CreateMap<Report,ReportDto>().AfterMap((src, dest) =>{
                var serilizationObj = new { OrganisationName = src.OrganisationName, Questions = "", Submitted = src.SubmittedDetails };

                dest.ReportingData = JsonConvert.SerializeObject(serilizationObj);
            });
        }
    }
}
