using Microsoft.AspNetCore.Mvc;

namespace ClaimManagementSystem.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
