using Microsoft.AspNetCore.Mvc;

namespace SaleSystem.WebApplication.Controllers
{
    public class PlantillaController : Controller
    {
        public IActionResult EnviarClave(string correo, string clave)
        {
            ViewData["Correo"] = correo;
            ViewData["Clave"] = clave;
            ViewData["Url"] = $"{this.Request.Scheme}://{this.Request.Host}";

            return View();
        }

        public async Task<IActionResult> PDFVenta(string numeroVenta)
        {

            //VMVenta vmVenta = _mapper.Map<VMVenta>(await _ventaServicio.Detalle(numeroVenta));
            //VMNegocio vmNegocio = _mapper.Map<VMNegocio>(await _negocioServicio.Obtener());

            //VMPDFVenta modelo = new VMPDFVenta();

            //modelo.negocio = vmNegocio;
            //modelo.venta = vmVenta;

            return View();
        }

        public IActionResult RestablecerClave(string clave)
        {
            ViewData["Clave"] = clave;
            return View();
        }
    }
}
