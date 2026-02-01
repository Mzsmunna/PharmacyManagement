using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class BillingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details()
        {
            return View();
        }
    }
}
