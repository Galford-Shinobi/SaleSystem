using Microsoft.AspNetCore.Mvc;

namespace SaleSystem.WebApplication.Controllers
{
    public class NegocioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
