using SFA.DAS.PSRService.Domain.Entities;
using SFA.DAS.PSRService.Web.Models;

namespace SFA.DAS.PSRService.Web.ViewModels
{
    public class TotalEmployees
    {
        public Report Report { get; set; }
        public bool? HasTotalEmployeesMeetMinimum { get; set; }
    }
}
