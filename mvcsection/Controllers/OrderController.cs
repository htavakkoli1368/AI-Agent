using Microsoft.AspNetCore.Mvc;

namespace mvcsection.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
