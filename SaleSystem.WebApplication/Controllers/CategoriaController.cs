using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SaleSystem.BLL.Interfaces;
using SaleSystem.Entity.Entities;
using SaleSystem.WebApplication.Models.ViewModels;
using SaleSystem.WebApplication.Utilidades.Respuestas;

namespace SaleSystem.WebApplication.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ICategoriaService _categoriaServicio;

        public CategoriaController(IMapper mapper, ICategoriaService categoriaServicio)
        {
            _mapper = mapper;
            _categoriaServicio = categoriaServicio;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            var listResult = await _categoriaServicio.ListaAsync();

            if (!listResult.IsSuccess)
            {
                return StatusCode(StatusCodes.Status404NotFound, new { data = listResult });
            }

            List<VMCategoria> vmCategoriaLista = _mapper.Map<List<VMCategoria>>(listResult.ListObjet);
            return StatusCode(StatusCodes.Status200OK, new { data = vmCategoriaLista });
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] VMCategoria modelo)
        {
            var categoria_creada = await _categoriaServicio.CrearAsync(_mapper.Map<Categoria>(modelo));

            if (!categoria_creada.IsSuccess)
            {
                return StatusCode(StatusCodes.Status404NotFound, categoria_creada);
            }

            modelo = _mapper.Map<VMCategoria>(categoria_creada.DirectObject);

            return StatusCode(StatusCodes.Status200OK, categoria_creada);
        }

        [HttpPut]
        public async Task<IActionResult> Editar([FromBody] VMCategoria modelo)
        {
            RespuestaGenerica<VMCategoria> gRespuesta = new RespuestaGenerica<VMCategoria>();

            var gResponse = await _categoriaServicio.EditarAsync(_mapper.Map<Categoria>(modelo));


            if (!gResponse.IsSuccess)
            {
                gRespuesta.Estado = gResponse.IsSuccess;
                gRespuesta.Mensaje = gResponse.ErrorMessage;
                return StatusCode(StatusCodes.Status200OK, gRespuesta);
            }
            modelo = _mapper.Map<VMCategoria>(gResponse.DirectObject);
            gRespuesta.Estado = gResponse.IsSuccess;
            gRespuesta.Objeto = modelo;
            return StatusCode(StatusCodes.Status200OK, gRespuesta);
        }

        [HttpDelete]
        public async Task<IActionResult> Eliminar(int IdCategoria)
        {
            RespuestaGenerica<VMCategoria> gRespuesta = new RespuestaGenerica<VMCategoria>();
            var gResponse = await _categoriaServicio.EliminarAsync(IdCategoria);
            if (!gResponse.IsSuccess)
            {
                gRespuesta.Estado = gResponse.IsSuccess;
                gRespuesta.Mensaje = gResponse.ErrorMessage;
                return StatusCode(StatusCodes.Status200OK, gRespuesta);
            }
            gRespuesta.Estado = gResponse.IsSuccess;
            gRespuesta.Objeto = _mapper.Map<VMCategoria>(gResponse.DirectObject);
            return StatusCode(StatusCodes.Status200OK, gRespuesta);
        }

    }
}
