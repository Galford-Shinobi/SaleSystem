using SaleSystem.Entity.Entities;

namespace SaleSystem.BLL.Interfaces
{
    public interface IRolService
    {
        Task<List<Rol>> Lista();
    }
}
