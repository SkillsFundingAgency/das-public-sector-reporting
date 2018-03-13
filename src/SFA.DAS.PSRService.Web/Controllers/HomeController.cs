using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.PSRService.Web.Models;

namespace SFA.DAS.PSRService.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Submit(string action)
        {
            if (action == "Create")
                return new RedirectResult(Url.Action("Create", "Report"));

            return new RedirectResult(Url.Action("List", "Report"));
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
