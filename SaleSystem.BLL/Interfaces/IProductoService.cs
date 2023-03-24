using SaleSystem.Common.Response;
using SaleSystem.Entity.Entities;

namespace SaleSystem.BLL.Interfaces
{
    public interface IProductoService
    {
        Task<GenericResponse<Producto>> ListaAsync();
        Task<GenericResponse<Producto>> CrearAsync(Producto entidad, Stream imagen = null, string NombreImagen = "");
        Task<GenericResponse<Producto>> EditarAsync(Producto entidad, Stream imagen = null, string NombreImagen = "");
        Task<GenericResponse<object>> EliminarAsync(int idProducto);

    }
}
