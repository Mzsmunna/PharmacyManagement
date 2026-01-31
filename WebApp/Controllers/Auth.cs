using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class Auth : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
