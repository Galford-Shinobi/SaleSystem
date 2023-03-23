using Microsoft.EntityFrameworkCore;
using SaleSystem.Common.Response;
using SaleSystem.DAL.DBContext;
using SaleSystem.DAL.Interfaces;
using SaleSystem.Entity.Entities;

namespace SaleSystem.DAL.Implementacion
{
    public class VentaRepository : GenericRepository<Venta>, IVentaRepository
    {
        private readonly DBVENTAContext _dbContext;

        public VentaRepository(DBVENTAContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<GenericResponse<Venta>> Registrar(Venta entidad)
        {
            Venta ventaGenerada = new Venta();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {

                    foreach (DetalleVenta dv in entidad.DetalleVenta)
                    {

                        Producto producto_encontrado = _dbContext.Productos.Where(p => p.IdProducto == dv.IdProducto).First();

                        producto_encontrado.Stock = producto_encontrado.Stock - dv.Cantidad;
                        _dbContext.Productos.Update(producto_encontrado);
                    }
                    await _dbContext.SaveChangesAsync();


                    NumeroCorrelativo correlativo = _dbContext.NumeroCorrelativos.Where(n => n.Gestion == "venta").First();

                    correlativo.UltimoNumero = correlativo.UltimoNumero + 1;
                    correlativo.FechaActualizacion = DateTime.Now;

                    _dbContext.NumeroCorrelativos.Update(correlativo);
                    await _dbContext.SaveChangesAsync();


                    string ceros = string.Concat(Enumerable.Repeat("0", correlativo.CantidadDigitos!.Value));
                    string numeroVenta = ceros + correlativo.UltimoNumero.ToString();
                    numeroVenta = numeroVenta.Substring(numeroVenta.Length - correlativo.CantidadDigitos.Value, correlativo.CantidadDigitos.Value);

                    entidad.NumeroVenta = numeroVenta;

                    await _dbContext.Venta.AddAsync(entidad);
                    await _dbContext.SaveChangesAsync();

                    ventaGenerada = entidad;

                    transaction.Commit();

                    return new GenericResponse<Venta> { IsSuccess = true, DirectObject = ventaGenerada };

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new GenericResponse<Venta> { IsSuccess = false, ErrorMessage = ex.InnerException!.Message };
                }
            }

        }

        public async Task<GenericResponse<DetalleVenta>> Reporte(DateTime FechaInicio, DateTime FechaFin)
        {

            try
            {
                List<DetalleVenta> listaResumen = await _dbContext.DetalleVenta
               .Include(v => v.IdVentaNavigation)
               .ThenInclude(u => u!.IdUsuarioNavigation)
               .Include(v => v.IdVentaNavigation)
               .ThenInclude(tdv => tdv!.IdTipoDocumentoVentaNavigation)
               .Where(dv => dv.IdVentaNavigation!.FechaRegistro!.Value.Date >= FechaInicio.Date &&
                   dv.IdVentaNavigation.FechaRegistro.Value.Date <= FechaFin.Date).ToListAsync();


                return new GenericResponse<DetalleVenta> { IsSuccess = true, ListObjet = listaResumen };

            }
            catch (Exception exception)
            {

                return new GenericResponse<DetalleVenta> { IsSuccess = false, ErrorMessage = exception.InnerException!.Message };
            }

        }
    }
}
