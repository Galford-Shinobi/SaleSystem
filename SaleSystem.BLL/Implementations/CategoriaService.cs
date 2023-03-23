using SaleSystem.BLL.Interfaces;
using SaleSystem.Common.Response;
using SaleSystem.DAL.Interfaces;
using SaleSystem.Entity.Entities;

namespace SaleSystem.BLL.Implementations
{
    public class CategoriaService : ICategoriaService
    {
        private readonly IGenericRepository<Categoria> _repositorio;

        public CategoriaService(IGenericRepository<Categoria> repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<GenericResponse<Categoria>> CrearAsync(Categoria entidad)
        {
            try
            {
                var categoria_creada = await _repositorio.CrearAsync(entidad);
                if (categoria_creada is null)
                    //throw new TaskCanceledException("No se pudo crear la categoria");
                    return new GenericResponse<Categoria> { IsSuccess = false, ErrorMessage = "No se pudo crear la categoria" };

                return new GenericResponse<Categoria> { IsSuccess = true, DirectObject = categoria_creada };
            }
            catch (Exception exception)
            {
                return new GenericResponse<Categoria> { IsSuccess = false, ErrorMessage = exception.Message };
            }
        }

        public async Task<GenericResponse<Categoria>> EditarAsync(Categoria entidad)
        {
            try
            {
                var categoria_encontrada = await _repositorio.ObtenerAsync(c => c.IdCategoria == entidad.IdCategoria);
                categoria_encontrada.Descripcion = entidad.Descripcion;
                categoria_encontrada.EsActivo = entidad.EsActivo;

                var respuesta = await _repositorio.EditarAsync(categoria_encontrada);

                if (!respuesta)
                    //throw new TaskCanceledException("No se pudo modificar la categoria");
                    return new GenericResponse<Categoria> { IsSuccess = false, ErrorMessage = "No se pudo modificar la categoria" };


                return new GenericResponse<Categoria> { IsSuccess = true, DirectObject = categoria_encontrada };
            }
            catch (Exception exception)
            {
                return new GenericResponse<Categoria> { IsSuccess = false, ErrorMessage = exception.Message };
            }
        }

        public async Task<GenericResponse<object>> EliminarAsync(int idCategoria)
        {
            try
            {
                var categoria_encontrada = await _repositorio.ObtenerAsync(c => c.IdCategoria == idCategoria);

                if (categoria_encontrada is null)
                    //throw new TaskCanceledException("La categoria no existe");
                    return new GenericResponse<object> { IsSuccess = false, ErrorMessage = "La categoria no existe" };

                

                var respuesta = await _repositorio.EliminarAsync(categoria_encontrada!);

                if (!respuesta)
                    return new GenericResponse<object> { IsSuccess = false, ErrorMessage = "Error Mirror!" };

                return new GenericResponse<object> { IsSuccess = true, };
            }
            catch (Exception exception)
            {
                return new GenericResponse<object> { IsSuccess = false, ErrorMessage = exception.Message };
            }
        }

        public async Task<GenericResponse<Categoria>> ListaAsync()
        {
            try
            {
                var query = await _repositorio.ConsultarAsync();

                if (query.ToList().Count == 0 || query is null)
                {
                    return new GenericResponse<Categoria> { IsSuccess = false, ErrorMessage = "No Data!" };
                }

                return new GenericResponse<Categoria> { IsSuccess = true, ListObjet = query.ToList() };
            }
            catch (Exception exception)
            {
                return new GenericResponse<Categoria> { IsSuccess = false, ErrorMessage = exception.Message };
            }
        }
    }
}
