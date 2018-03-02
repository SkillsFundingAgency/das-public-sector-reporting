namespace SFA.DAS.AssessorService.Application.Domain
{
    using AssessorService.Domain.Enums;

    public class OrganisationUpdateDomainModel
    {
        public string EndPointAssessorOrganisationId { get; set; }
     
        public string EndPointAssessorName { get; set; }
        public string PrimaryContact { get; set; }
        public OrganisationStatus OrganisationStatus { get; set; }
    }
}
