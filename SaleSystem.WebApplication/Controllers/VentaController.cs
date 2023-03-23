using Microsoft.AspNetCore.Mvc;

namespace SaleSystem.WebApplication.Controllers
{
    public class VentaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult NuevaVenta()
        {
            return View();
        }

        public IActionResult HistorialVenta()
        {
            return View();
        }
    }
}
