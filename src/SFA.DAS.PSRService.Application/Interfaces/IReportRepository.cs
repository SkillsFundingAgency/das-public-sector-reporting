using SFA.DAS.PSRService.Api.Types.Models;

namespace SFA.DAS.PSRService.Application.Interfaces
{
    using System.Threading.Tasks;
    using Domain;

    public interface IReportRepository
    {       
        Task<Report> CreateNewContact(ReportCreateDomainModel newContact);
        Task Update(UpdateReportRequest organisationUpdateViewModel);
        Task Delete(string userName);
    }
}