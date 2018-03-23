using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Models;
using SFA.DAS.PSRService.Web.Models.Home;
using SFA.DAS.PSRService.Web.Services;

namespace SFA.DAS.PSRService.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IReportService _reportService;
        //private const string EmployerId = "ABCDE"; // TODO: get this from context

        public HomeController(IReportService reportService)
        {
            _reportService = reportService;
        }

        public IActionResult Index()
        {
            var model = new IndexViewModel();
            var period = _reportService.GetCurrentReportPeriod();
            var employerDetail = (EmployerIdentifier)HttpContext.Items[ContextItemKeys.EmployerIdentifier];
            var report = _reportService.GetReport(period, employerDetail.AccountId);
            model.PeriodName = _reportService.GetCurrentReportPeriodName(period);
            model.CanCreateReport = report == null;
            model.CanEditReport = report != null && !report.Submitted;
            return View(model);
        }

        public IActionResult Submit(string action)
        {
            if (action == "create")
                return new RedirectResult(Url.Action("Create", "Report"));
            if (action == "edit")
                return new RedirectResult(Url.Action("Edit", "Report"));

            if (action == "list")
                return new RedirectResult(Url.Action("List", "Report"));

            return new BadRequestResult();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("Cookies");
            await HttpContext.SignOutAsync("oidc");

            return Redirect("https://www.google.co.uk");
        }

        [Authorize]
        public IActionResult Protected(string empolyerId)
        {
            var employerDetail = (EmployerIdentifier)HttpContext.Items[ContextItemKeys.EmployerIdentifier];
            return View(employerDetail);
        }
    }
}
