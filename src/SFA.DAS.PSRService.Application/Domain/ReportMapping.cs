using System;
using System.Collections.Generic;
using System.Text;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Application.Domain
{
    public class ReportMapping
    {
    
        public IEnumerable<Section> Questions { get; set; }
        public string OrganisationName { get; set; }
        public Submitted Submitted { get; set; }
    }
}
