using Microsoft.EntityFrameworkCore;
using SaleSystem.DAL.DBContext;
using SaleSystem.DAL.Interfaces;
using System.Linq.Expressions;

namespace SaleSystem.DAL.Implementacion
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly DBVENTAContext _dbContext;

        public GenericRepository(DBVENTAContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TEntity> ObtenerAsync(Expression<Func<TEntity, bool>> filtro)
        {
            try
            {
                TEntity entidad = await _dbContext.Set<TEntity>().FirstOrDefaultAsync(filtro);
                return entidad;
            }
            catch
            {
                throw;
            }
        }

        public async Task<TEntity> CrearAsync(TEntity entidad)
        {
            try
            {
                _dbContext.Set<TEntity>().Add(entidad);
                await _dbContext.SaveChangesAsync();
                return entidad;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> EditarAsync(TEntity entidad)
        {
            try
            {
                _dbContext.Update(entidad);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> EliminarAsync(TEntity entidad)
        {
            try
            {
                _dbContext.Remove(entidad);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }
        public async Task<IQueryable<TEntity>> ConsultarAsync(Expression<Func<TEntity, bool>> filtro = null!)
        {
            IQueryable<TEntity> queryEntidad = filtro == null ? _dbContext.Set<TEntity>() : _dbContext.Set<TEntity>().Where(filtro);
            return queryEntidad;
        }

    }
}
