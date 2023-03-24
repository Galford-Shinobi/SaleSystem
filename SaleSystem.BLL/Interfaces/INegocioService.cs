using SaleSystem.Common.Response;
using SaleSystem.Entity.Entities;

namespace SaleSystem.BLL.Interfaces
{
    public interface INegocioService
    {
        Task<GenericResponse<Negocio>> ObtenerAsync();
        Task<GenericResponse<Negocio>> GuardarCambiosAsync(Negocio entidad, Stream Logo = null, string NombreLogo = "");

    }
}
