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

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult NotRegistered()
        {
            return View();
        }

        public IActionResult InvalidRole()
        {
            return View();
        }
    }
}
