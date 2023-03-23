using Microsoft.AspNetCore.Mvc;

namespace SaleSystem.WebApplication.Controllers
{
    public class UsuarioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
