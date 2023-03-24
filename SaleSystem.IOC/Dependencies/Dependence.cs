using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SaleSystem.BLL.Implementations;
using SaleSystem.BLL.Interfaces;
using SaleSystem.DAL.DBContext;
using SaleSystem.DAL.Implementacion;
using SaleSystem.DAL.Interfaces;


namespace SaleSystem.IOC.Dependencies
{
    public static class Dependence
    {
        public static void InyectarDependencia(this IServiceCollection services, IConfiguration Configuration) {
            services.AddDbContext<DBVENTAContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultSqlSConnection"));
            });
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IVentaRepository, VentaRepository>();

            services.AddScoped<ICategoriaService, CategoriaService>();
            services.AddScoped<IProductoService, ProductoService>();
            services.AddScoped<IRolService, RolService>();

            services.AddScoped<IUtilidadesService, UtilidadesService>();
            services.AddScoped<ICorreoService, CorreoService>();
            services.AddScoped<IFireBaseService, FireBaseService>();
        }
    }
}
