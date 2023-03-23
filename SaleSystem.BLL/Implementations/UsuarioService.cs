using Microsoft.EntityFrameworkCore;
using SaleSystem.BLL.Interfaces;
using SaleSystem.Common.Response;
using SaleSystem.DAL.Interfaces;
using SaleSystem.Entity.Entities;


namespace SaleSystem.BLL.Implementations
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IGenericRepository<Usuario> _repositorio;
        private readonly IFireBaseService _fireBaseService;
        private readonly IUtilidadesService _utilidadesService;
        private readonly ICorreoService _correoService;

        public UsuarioService(
            IGenericRepository<Usuario> repositorio,
            IFireBaseService fireBaseService,
            IUtilidadesService utilidadesService,
            ICorreoService correoService)
        {
            _repositorio = repositorio;
            _fireBaseService = fireBaseService;
            _utilidadesService = utilidadesService;
            _correoService = correoService;
        }

        public Task<GenericResponse<object>> CambiarClaveAsync(int IdUsuario, string ClaveActual, string ClaveNueva)
        {
            throw new NotImplementedException();
        }

        public Task<GenericResponse<Usuario>> CrearAsync(Usuario entidad, Stream Foto = null, string NombreFoto = "", string UrlPlantillaCorreo = "")
        {
            throw new NotImplementedException();
        }

        public Task<GenericResponse<Usuario>> EditarAsync(Usuario entidad, Stream Foto = null, string NombreFoto = "")
        {
            throw new NotImplementedException();
        }

        public Task<GenericResponse<object>> EliminarAsync(int IdUsuario)
        {
            throw new NotImplementedException();
        }

        public Task<GenericResponse<object>> GuardarPefilAsync(Usuario entidad)
        {
            throw new NotImplementedException();
        }

        public async Task<GenericResponse<Usuario>> ListaAsync()
        {
            try
            {
                var query = await _repositorio.ConsultarAsync();
                

                return new GenericResponse<Usuario> { IsSuccess = true, ListObjet = query.Include(r => r.IdRolNavigation).ToList() };
            }
            catch (Exception exception)
            {
                return new GenericResponse<Usuario> { IsSuccess = false, ErrorMessage = exception.InnerException!.Message };
            }
        }

        public Task<GenericResponse<Usuario>> ObtenerPorCredencialesAsync(string correo, string clave)
        {
            throw new NotImplementedException();
        }

        public Task<GenericResponse<Usuario>> ObtenerPorIdAsync(int IdUsuario)
        {
            throw new NotImplementedException();
        }

        public Task<GenericResponse<object>> RestablecerClaveAsync(string Correo, string UrlPlantillaCorreo)
        {
            throw new NotImplementedException();
        }
    }
}
