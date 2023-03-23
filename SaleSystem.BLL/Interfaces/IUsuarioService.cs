using SaleSystem.Common.Response;
using SaleSystem.Entity.Entities;

namespace SaleSystem.BLL.Interfaces
{
    public interface IUsuarioService
    {
        Task<GenericResponse<Usuario>> ListaAsync();
        Task<GenericResponse<Usuario>> CrearAsync(Usuario entidad, Stream Foto = null, string NombreFoto = "", string UrlPlantillaCorreo = "");
        Task<GenericResponse<Usuario>> EditarAsync(Usuario entidad, Stream Foto = null, string NombreFoto = "");

        Task<GenericResponse<object>> EliminarAsync(int IdUsuario);

        Task<GenericResponse<Usuario>> ObtenerPorCredencialesAsync(string correo, string clave);

        Task<GenericResponse<Usuario>> ObtenerPorIdAsync(int IdUsuario);

        Task<GenericResponse<object>> GuardarPefilAsync(Usuario entidad);

        Task<GenericResponse<object>> CambiarClaveAsync(int IdUsuario, string ClaveActual, string ClaveNueva);

        Task<GenericResponse<object>> RestablecerClaveAsync(string Correo, string UrlPlantillaCorreo);
    }
}
