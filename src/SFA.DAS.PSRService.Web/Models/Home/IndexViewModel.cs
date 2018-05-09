using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.PSRService.Domain.Entities;

namespace SFA.DAS.PSRService.Web.Models.Home
{
    public class IndexViewModel
    {
        public bool CanCreateReport { get;set; }
        public bool CanEditReport { get; set; }
        public Period Period { get; set; }
    }
}
