using Microsoft.AspNetCore.Mvc;

namespace ShoppingProject.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
