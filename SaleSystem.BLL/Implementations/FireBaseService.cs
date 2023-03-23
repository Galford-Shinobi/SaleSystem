using Firebase.Auth;
using Firebase.Storage;
using SaleSystem.BLL.Interfaces;
using SaleSystem.DAL.Interfaces;
using SaleSystem.Entity.Entities;


namespace SaleSystem.BLL.Implementations
{
    public class FireBaseService : IFireBaseService
    {
        private readonly IGenericRepository<Configuracion> _repositorio;

        public FireBaseService(IGenericRepository<Configuracion> repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<bool> EliminarStorageAsync(string CarpetaDestino, string NombreArchivo)
        {
            try
            {
                //IQueryable<Configuracion> query = await _repositorio.ConsultarAsync(c => c.Recurso.Equals("FireBase_Storage"));

                var FireBasequery = await _repositorio.ConsultarAsync(c => c.Recurso!.Equals("FireBase_Storage"));

                if (FireBasequery is null)
                {
                    return false;
                }


                Dictionary<string, string> Config = FireBasequery.ToDictionary(keySelector: c => c.Propiedad, elementSelector: c => c.Valor)!;


                var auth = new FirebaseAuthProvider(new FirebaseConfig(Config["api_key"]));
                var a = await auth.SignInWithEmailAndPasswordAsync(Config["email"], Config["clave"]);

                var cancellation = new CancellationTokenSource();

                var task = new FirebaseStorage(
                    Config["ruta"],
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                        ThrowOnCancel = true
                    })
                    .Child(Config[CarpetaDestino])
                    .Child(NombreArchivo)
                    .DeleteAsync();

                await task;

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<string> SubirStorageAsync(Stream StreamArchivo, string CarpetaDestino, string NombreArchivo)
        {
            string UrlImagen = "";
            try
            {
                //IQueryable<Configuracion> query = await _repositorio.ConsultarAsync(c => c.Recurso.Equals("FireBase_Storage"));
                var FireBasequery = await _repositorio.ConsultarAsync(c => c.Recurso!.Equals("FireBase_Storage"));

                if (FireBasequery is null)
                {
                    return string.Empty;
                }

                Dictionary<string, string> Config = FireBasequery.ToDictionary(keySelector: c => c.Propiedad, elementSelector: c => c.Valor)!;


                var auth = new FirebaseAuthProvider(new FirebaseConfig(Config["api_key"]));
                var a = await auth.SignInWithEmailAndPasswordAsync(Config["email"], Config["clave"]);

                var cancellation = new CancellationTokenSource();

                var task = new FirebaseStorage(
                    Config["ruta"],
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                        ThrowOnCancel = true
                    })
                    .Child(Config[CarpetaDestino])
                    .Child(NombreArchivo)
                    .PutAsync(StreamArchivo, cancellation.Token);

                UrlImagen = await task;
            }
            catch (Exception)
            {
                UrlImagen = "";
            }
            return UrlImagen;
        }
    }
}
