using SaleSystem.BLL.Interfaces;
using SaleSystem.Common.Response;
using SaleSystem.DAL.Interfaces;
using SaleSystem.Entity.Entities;

namespace SaleSystem.BLL.Implementations
{
    public class NegocioService : INegocioService
    {
        private readonly IGenericRepository<Negocio> _repositorio;
        private readonly IFireBaseService _firebaseService;

        public NegocioService(IGenericRepository<Negocio> repositorio, IFireBaseService firebaseService)
        {
            _repositorio = repositorio;
            _firebaseService = firebaseService;
        }

        public async Task<GenericResponse<Negocio>> ObtenerAsync()
        {
            try
            {
                Negocio negocio_encontrado = await _repositorio.ObtenerAsync(n => n.IdNegocio == 1);


                return new GenericResponse<Negocio> { IsSuccess = true, DirectObject = negocio_encontrado };
            }
            catch(Exception exception)
            {
                return new GenericResponse<Negocio> { IsSuccess = false, ErrorMessage = exception.Message};
            }
        }
        public async Task<GenericResponse<Negocio>> GuardarCambiosAsync(Negocio entidad, Stream Logo = null, string NombreLogo = "")
        {
            try
            {
                Negocio negocio_encontrado = await _repositorio.ObtenerAsync(n => n.IdNegocio == 1);


                negocio_encontrado.NumeroDocumento = entidad.NumeroDocumento;
                negocio_encontrado.Nombre = entidad.Nombre;
                negocio_encontrado.Correo = entidad.Correo;
                negocio_encontrado.Direccion = entidad.Direccion;
                negocio_encontrado.Telefono = entidad.Telefono;
                negocio_encontrado.PorcentajeImpuesto = entidad.PorcentajeImpuesto;
                negocio_encontrado.SimboloMoneda = entidad.SimboloMoneda;

                negocio_encontrado.NombreLogo = negocio_encontrado.NombreLogo == "" ? NombreLogo : negocio_encontrado.NombreLogo;

                if (Logo != null)
                {
                    string urlLogo = await _firebaseService.SubirStorageAsync(Logo, "carpeta_logo", negocio_encontrado.NombreLogo!);
                    negocio_encontrado.UrlLogo = urlLogo;

                }

                await _repositorio.EditarAsync(negocio_encontrado);
                return new GenericResponse<Negocio> { IsSuccess = true, DirectObject = negocio_encontrado };
            }
            catch(Exception exception)
            {
               return new GenericResponse<Negocio> { IsSuccess = false , ErrorMessage = exception.Message};
            }

        }

    }
}
