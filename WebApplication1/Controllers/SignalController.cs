using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class SignalController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}