using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.PSRService.Web.Configuration;
using SFA.DAS.PSRService.Web.Models;

namespace SFA.DAS.PSRService.Web.Controllers
{
    [Authorize]
    //[Route("accounts/{employerAccountId}")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Submit(string action)
        {
            if (action == "create")
                return new RedirectResult(Url.Action("Create", "Report"));

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
        public IActionResult Protected()
        {
            var employerDetail = (EmployerIdentifier)HttpContext.Items[ContextItemKeys.EmployerIdentifier];
            return View(employerDetail);
        }
    }
}
