using StructureMap.Query;

namespace SFA.DAS.PSRService.Web.ViewModels
{
    public class OrganisationReviewViewModel
    {
        public string OrganisationName { get; set; }
        public bool? HasMinimumEmployeeHeadcount { get; set; }
        public bool? IsLocalAuthority { get; set; }
    }
}
