

using SaleSystem.Common.Response;
using SaleSystem.Entity.Entities;

namespace SaleSystem.DAL.Interfaces
{
    public interface IVentaRepository : IGenericRepository<Venta>
    {
        Task<GenericResponse<Venta>> Registrar(Venta entidad);
        Task<GenericResponse<DetalleVenta>> Reporte(DateTime FechaInicio, DateTime FechaFin);
    }
}
