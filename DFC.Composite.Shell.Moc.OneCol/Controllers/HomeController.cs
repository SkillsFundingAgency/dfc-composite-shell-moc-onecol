using System.Diagnostics;
using DFC.Composite.Shell.Moc.OneCol.Models;
using Microsoft.AspNetCore.Mvc;

namespace DFC.Composite.Shell.Moc.OneCol.Controllers
{
    public class HomeController : Controller
    {
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
