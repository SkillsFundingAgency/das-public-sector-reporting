using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.PSRService.Web.Models;

namespace SFA.DAS.PSRService.Web.Controllers
{
    [Authorize]
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

        [Authorize]
        public IActionResult Protected()
        {
            return View();
        }
    }
}
