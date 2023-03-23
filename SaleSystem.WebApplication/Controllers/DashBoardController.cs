using Microsoft.AspNetCore.Mvc;

namespace SaleSystem.WebApplication.Controllers
{
    public class DashBoardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
