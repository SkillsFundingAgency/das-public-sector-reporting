using System.Threading.Tasks;
using SFA.DAS.PSRService.Web.ViewModels;

namespace SFA.DAS.PSRService.Web.Services
{
    public interface IOrganisationService
    {
        Task<Organisation> GetOrganisation(string token, int ukprn);
    }
}