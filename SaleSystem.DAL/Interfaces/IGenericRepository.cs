using System.Linq.Expressions;

namespace SaleSystem.DAL.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<TEntity> ObtenerAsync(Expression<Func<TEntity, bool>> filtro);
        Task<TEntity> CrearAsync(TEntity entidad);
        Task<bool> EditarAsync(TEntity entidad);
        Task<bool> EliminarAsync(TEntity entidad);
        Task<IQueryable<TEntity>> ConsultarAsync(Expression<Func<TEntity, bool>> filtro = null);

    }
}
