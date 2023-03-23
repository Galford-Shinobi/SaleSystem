using SaleSystem.BLL.Interfaces;
using SaleSystem.DAL.Interfaces;
using SaleSystem.Entity.Entities;

namespace SaleSystem.BLL.Implementations
{
    public class RolService : IRolService
    {
        private readonly IGenericRepository<Rol> _repositorio;

        public RolService(IGenericRepository<Rol> repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<List<Rol>> Lista()
        {

            var Rolquery = await _repositorio.ConsultarAsync();

            if (Rolquery.ToList().Count == 0)
            {
                return new List<Rol>();
            }

            return Rolquery.ToList();

        }
    }
}
