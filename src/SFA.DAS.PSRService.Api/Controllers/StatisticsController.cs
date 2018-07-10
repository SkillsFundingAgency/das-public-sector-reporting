using System.Web.Http;
using SFA.DAS.PSRService.Api.Attributes;

namespace SFA.DAS.PSRService.Api.Controllers
{
    [ApiAuthorize(Roles = "ReadUserAccounts")]
    [RoutePrefix("api/statistics")]
    public class StatisticsController : ApiController
    {
        public StatisticsController()
        {
            
        }

        [Route("")]
        public void GetStatistics()
        {
        }
    }
}
