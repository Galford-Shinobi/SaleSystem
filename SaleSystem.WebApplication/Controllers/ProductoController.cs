using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SaleSystem.BLL.Interfaces;
using SaleSystem.Common.Response;
using SaleSystem.Entity.Entities;
using SaleSystem.WebApplication.Models.ViewModels;
using SaleSystem.WebApplication.Utilidades.Respuestas;

namespace SaleSystem.WebApplication.Controllers
{
    public class ProductoController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IProductoService _productoServicio;

        public ProductoController(IMapper mapper,
            IProductoService productoServicio)
        {
            _mapper = mapper;
            _productoServicio = productoServicio;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            var ResultPrtoduct = await _productoServicio.ListaAsync();

            if (!ResultPrtoduct.IsSuccess)
            {
                return StatusCode(StatusCodes.Status200OK, new { data = new List<VMProducto>() });
            }

            List<VMProducto> vmProductoLista = _mapper.Map<List<VMProducto>>(ResultPrtoduct.ListObjet);

            return StatusCode(StatusCodes.Status200OK, new { data = vmProductoLista });
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromForm] IFormFile imagen, [FromForm] string modelo)
        {

            RespuestaGenerica<VMProducto> gResponse = new RespuestaGenerica<VMProducto>();

            try
            {
                VMProducto vmProducto = JsonConvert.DeserializeObject<VMProducto>(modelo);

                string nombreImagen = "";
                Stream imagenStream = null;

                if (imagen != null)
                {
                    string nombre_en_codigo = Guid.NewGuid().ToString("N");
                    string extension = Path.GetExtension(imagen.FileName);
                    nombreImagen = string.Concat(nombre_en_codigo, extension);
                    imagenStream = imagen.OpenReadStream();
                }

                var producto_creado = await _productoServicio.CrearAsync(_mapper.Map<Producto>(vmProducto), imagenStream!, nombreImagen);

                if (!producto_creado.IsSuccess)
                    gResponse.Estado = producto_creado.IsSuccess;
                    gResponse.Mensaje = producto_creado.ErrorMessage;

                vmProducto = _mapper.Map<VMProducto>(producto_creado.DirectObject);

                gResponse.Estado = true;
                gResponse.Objeto = vmProducto;

            }
            catch (Exception ex)
            {
                gResponse.Estado = false;
                gResponse.Mensaje = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

        [HttpPut]
        public async Task<IActionResult> Editar([FromForm] IFormFile imagen, [FromForm] string modelo)
        {

            RespuestaGenerica<VMProducto> gResponse = new RespuestaGenerica<VMProducto>();

            try
            {
                VMProducto vmProducto = JsonConvert.DeserializeObject<VMProducto>(modelo);

                string nombreImagen = "";
                Stream imagenStream = null;

                if (imagen != null)
                {
                    string nombre_en_codigo = Guid.NewGuid().ToString("N");
                    string extension = Path.GetExtension(imagen.FileName);
                    nombreImagen = string.Concat(nombre_en_codigo, extension);
                    imagenStream = imagen.OpenReadStream();
                }

                var  producto_editado = await _productoServicio.EditarAsync(_mapper.Map<Producto>(vmProducto), imagenStream, nombreImagen);

                vmProducto = _mapper.Map<VMProducto>(producto_editado.DirectObject);

                gResponse.Estado = true;
                gResponse.Objeto = vmProducto;

            }
            catch (Exception ex)
            {
                gResponse.Estado = false;
                gResponse.Mensaje = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

        [HttpDelete]
        public async Task<IActionResult> Eliminar(int IdProducto)
        {
            RespuestaGenerica<string> gResponse = new RespuestaGenerica<string>();

            try
            {

             var gResponseProduct = await _productoServicio.EliminarAsync(IdProducto);
                gResponse.Estado = gResponseProduct.IsSuccess;
                gResponse.Mensaje = gResponseProduct.ErrorMessage;
            }
            catch (Exception ex)
            {
                gResponse.Estado = false;
                gResponse.Mensaje = ex.Message;
            }
            return StatusCode(StatusCodes.Status200OK, gResponse);

        }
    }
}
