using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SaleSystem.BLL.Interfaces;
using SaleSystem.Common.Response;
using SaleSystem.DAL.Interfaces;
using SaleSystem.Entity.Entities;
using System.Net;
using System.Text;

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

        public async Task<GenericResponse<object>> CambiarClaveAsync(int IdUsuario, string ClaveActual, string ClaveNueva)
        {
            try
            {
                Usuario usuario_encontrado = await _repositorio.ObtenerAsync(u => u.IdUsuario == IdUsuario);

                if (usuario_encontrado == null)
                    throw new TaskCanceledException("El usuario no existe");

                if (usuario_encontrado.Clave != _utilidadesService.ConvertirSha256(ClaveActual))
                    throw new TaskCanceledException("La contraseña ingresada como actual no es correcta");

                usuario_encontrado.Clave = _utilidadesService.ConvertirSha256(ClaveNueva);

                bool respuesta = await _repositorio.EditarAsync(usuario_encontrado);

                if (!respuesta)
                    return new GenericResponse<object> { IsSuccess = false, ErrorMessage = "Noo Data" };

                return new GenericResponse<object> { IsSuccess = true, DirectObject = usuario_encontrado };


            }
            catch (Exception ex)
            {
                return new GenericResponse<object> { IsSuccess = false ,ErrorMessage = ex.InnerException!.Message };
            }
        }

        public async Task<GenericResponse<Usuario>> CrearAsync(Usuario entidad, Stream Foto = null, string NombreFoto = "", string UrlPlantillaCorreo = "")
        {
            Usuario usuario_existe = await _repositorio.ObtenerAsync(u => u.Correo == entidad.Correo);

            if (usuario_existe != null)
                throw new TaskCanceledException("El correo ya existe");


            try
            {

                string clave_generada = _utilidadesService.GenerarClave();
                entidad.Clave = _utilidadesService.ConvertirSha256(clave_generada);
                entidad.NombreFoto = NombreFoto;

                if (Foto != null)
                {
                    string urlFoto = await _fireBaseService.SubirStorageAsync(Foto, "carpeta_usuario", NombreFoto);
                    entidad.UrlFoto = urlFoto;
                }

                Usuario usuario_creado = await _repositorio.CrearAsync(entidad);

                if (usuario_creado.IdUsuario == 0)
                    throw new TaskCanceledException("No se pudo crear el usuario");

                if (UrlPlantillaCorreo != "")
                {
                    UrlPlantillaCorreo = UrlPlantillaCorreo.Replace("[correo]", usuario_creado.Correo).Replace("[clave]", clave_generada);

                    string htmlCorreo = "";

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(UrlPlantillaCorreo);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    if (response.StatusCode == HttpStatusCode.OK)
                    {

                        using (Stream dataStream = response.GetResponseStream())
                        {

                            StreamReader readerStream = null;

                            if (response.CharacterSet == null)
                                readerStream = new StreamReader(dataStream);
                            else
                                readerStream = new StreamReader(dataStream, Encoding.GetEncoding(response.CharacterSet));

                            htmlCorreo = readerStream.ReadToEnd();
                            response.Close();
                            readerStream.Close();


                        }
                    }

                    if (htmlCorreo != "")
                        await _correoService.EnviarCorreoAsync(usuario_creado.Correo!, "Cuenta Creada", htmlCorreo);
                }

                IQueryable<Usuario> query = await _repositorio.ConsultarAsync(u => u.IdUsuario == usuario_creado.IdUsuario);
                usuario_creado = query.Include(r => r.IdRolNavigation).First();

                return new GenericResponse<Usuario> { IsSuccess=true, DirectObject = usuario_creado };
            }
            catch (Exception ex)
            {
               return new GenericResponse<Usuario> { IsSuccess = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<GenericResponse<Usuario>> EditarAsync(Usuario entidad, Stream Foto = null, string NombreFoto = "")
        {
            Usuario usuario_existe = await _repositorio.ObtenerAsync(u => u.Correo == entidad.Correo && u.IdUsuario != entidad.IdUsuario);

            if (usuario_existe != null)
                throw new TaskCanceledException("El correo ya existe");


            try
            {

                IQueryable<Usuario> queryUsuario = await _repositorio.ConsultarAsync(u => u.IdUsuario == entidad.IdUsuario);

                Usuario usuario_editar = queryUsuario.First();

                usuario_editar.Nombre = entidad.Nombre;
                usuario_editar.Correo = entidad.Correo;
                usuario_editar.Telefono = entidad.Telefono;
                usuario_editar.IdRol = entidad.IdRol;
                usuario_editar.EsActivo = entidad.EsActivo;

                if (usuario_editar.NombreFoto == "")
                    usuario_editar.NombreFoto = NombreFoto;

                if (Foto != null)
                {
                    string urlFoto = await _fireBaseService.SubirStorageAsync(Foto, "carpeta_usuario", usuario_editar.NombreFoto!);
                    usuario_editar.UrlFoto = urlFoto;
                }

                bool respuesta = await _repositorio.EditarAsync(usuario_editar);

                if (!respuesta)
                    throw new TaskCanceledException("No se pudo modificar el usuario");

                Usuario usuario_editado = queryUsuario.Include(r => r.IdRolNavigation).First();

                return new GenericResponse<Usuario> { IsSuccess = true, DirectObject= usuario_editado };

            }
            catch(Exception exception)
            {
               return new GenericResponse<Usuario> { IsSuccess = false, ErrorMessage = exception.Message};
            }
        }

        public async Task<GenericResponse<object>> EliminarAsync(int IdUsuario)
        {
            try
            {
                Usuario usuario_encontrado = await _repositorio.ObtenerAsync(u => u.IdUsuario == IdUsuario);

                if (usuario_encontrado == null)
                    throw new TaskCanceledException("El usuario no existe");

                string nombreFoto = usuario_encontrado.NombreFoto!;
                bool respuesta = await _repositorio.EliminarAsync(usuario_encontrado);

                if (respuesta)
                    await _fireBaseService.EliminarStorageAsync("carpeta_usuario", nombreFoto);

                return new GenericResponse<object> { IsSuccess = true, };

            }
            catch(Exception exceptrion)
            {
               return  new GenericResponse<object> { IsSuccess = false, ErrorMessage = exceptrion.Message};
            }
        }

        public async Task<GenericResponse<object>> GuardarPefilAsync(Usuario entidad)
        {
            try
            {
                Usuario usuario_encontrado = await _repositorio.ObtenerAsync(u => u.IdUsuario == entidad.IdUsuario);

                if (usuario_encontrado == null)
                    throw new TaskCanceledException("El usuario no existe");


                usuario_encontrado.Correo = entidad.Correo;
                usuario_encontrado.Telefono = entidad.Telefono;

                bool respuesta = await _repositorio.EditarAsync(usuario_encontrado);

                if (respuesta)
                    return new GenericResponse<object> { IsSuccess = false, ErrorMessage = "No Data!"};

                return new GenericResponse<object> { IsSuccess = true};

            }
            catch(Exception ex)
            {
                return new GenericResponse<object> { IsSuccess = false, ErrorMessage = ex.Message};
            }
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

        public async Task<GenericResponse<Usuario>> ObtenerPorCredencialesAsync(string correo, string clave)
        {
            try
            {
                string clave_encriptada = _utilidadesService.ConvertirSha256(clave);

                Usuario usuario_encontrado = await _repositorio.ObtenerAsync(u => u.Correo!.Equals(correo)
                && u.Clave!.Equals(clave_encriptada));

                return new GenericResponse<Usuario> { IsSuccess = true, DirectObject = usuario_encontrado };
            }
            catch (Exception exception)
            {
               return new GenericResponse<Usuario> { IsSuccess=false, ErrorMessage = exception.Message};
            }
        }

        public async Task<GenericResponse<Usuario>> ObtenerPorIdAsync(int IdUsuario)
        {
            try
            {
                IQueryable<Usuario> query = await _repositorio.ConsultarAsync(u => u.IdUsuario == IdUsuario);

                Usuario resultado = await query.Include(r => r.IdRolNavigation!).FirstOrDefaultAsync();

                return new GenericResponse<Usuario> { IsSuccess = true, DirectObject = resultado! };
            }
            catch (Exception ex)
            {

               return new GenericResponse<Usuario> { IsSuccess = false, ErrorMessage = ex.Message} ;
            }
        }

        public async Task<GenericResponse<object>> RestablecerClaveAsync(string Correo, string UrlPlantillaCorreo)
        {
            try
            {
                Usuario usuario_encontrado = await _repositorio.ObtenerAsync(u => u.Correo == Correo);

                if (usuario_encontrado == null)
                    return new GenericResponse<object> { IsSuccess = false, ErrorMessage= "No encontramos ningún usuario asociado al correo" };
                    //throw new TaskCanceledException("No encontramos ningún usuario asociado al correo");


                string clave_generada = _utilidadesService.GenerarClave();
                usuario_encontrado.Clave = _utilidadesService.ConvertirSha256(clave_generada);


                UrlPlantillaCorreo = UrlPlantillaCorreo.Replace("[clave]", clave_generada);

                string htmlCorreo = "";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(UrlPlantillaCorreo);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {

                    using (Stream dataStream = response.GetResponseStream())
                    {

                        StreamReader readerStream = null;

                        if (response.CharacterSet == null)
                            readerStream = new StreamReader(dataStream);
                        else
                            readerStream = new StreamReader(dataStream, Encoding.GetEncoding(response.CharacterSet));

                        htmlCorreo = readerStream.ReadToEnd();
                        response.Close();
                        readerStream.Close();


                    }
                }

                GenericResponse<object> correo_enviado = new GenericResponse<object>();

                if (htmlCorreo != "")
                      correo_enviado = await _correoService.EnviarCorreoAsync(Correo, "Contraseña Restablecida", htmlCorreo);


                if (!correo_enviado.IsSuccess)
                     return new GenericResponse<object> { IsSuccess = false, ErrorMessage = "Tenemos problemas. Por favor inténtalo de nuevo más tarde" };
                    //throw new TaskCanceledException("Tenemos problemas. Por favor inténtalo de nuevo más tarde");
                    

                bool respuesta = await _repositorio.EditarAsync(usuario_encontrado);

                if (!respuesta)
                    return new GenericResponse<object> { IsSuccess = false, ErrorMessage = "Tenemos problemas. Por favor inténtalo de nuevo más tarde" };

                return new GenericResponse<object> { IsSuccess = true }; 

            }
            catch (Exception ex)
            {
                return new GenericResponse<object> { IsSuccess = false, ErrorMessage =  ex.Message};
            }

        }
    }
}
