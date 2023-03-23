using SaleSystem.Common.Response;
using SaleSystem.Entity.Entities;

namespace SaleSystem.BLL.Interfaces
{
    public interface ICategoriaService
    {

        Task<GenericResponse<Categoria>> ListaAsync();
        Task<GenericResponse<Categoria>> CrearAsync(Categoria entidad);
        Task<GenericResponse<Categoria>> EditarAsync(Categoria entidad);
        Task<GenericResponse<object>> EliminarAsync(int idCategoria);

    }
}
