using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.PSRService.Web.Models;

namespace SFA.DAS.PSRService.Web.Services
{
    public interface IEmployerAccountService
    {
        Task<IDictionary<string, EmployerIdentifier>> GetEmployerIdentifiersAsync(string userId);
    }
}
