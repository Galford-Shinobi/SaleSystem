using Microsoft.EntityFrameworkCore;
using SaleSystem.BLL.Interfaces;
using SaleSystem.Common.Response;
using SaleSystem.DAL.Interfaces;
using SaleSystem.Entity.Entities;

namespace SaleSystem.BLL.Implementations
{
    public class ProductoService : IProductoService
    {
        private readonly IGenericRepository<Producto> _repositorio;
        private readonly IFireBaseService _fireBaseServicio;

        public ProductoService(IGenericRepository<Producto> repositorio,
            IFireBaseService fireBaseServicio)
        {
            _repositorio = repositorio;
            _fireBaseServicio = fireBaseServicio;
        }

        public async Task<GenericResponse<Producto>> CrearAsync(Producto entidad, Stream imagen = null, string NombreImagen = "")
        {
            Producto producto_existe = await _repositorio.ObtenerAsync(p => p.CodigoBarra == entidad.CodigoBarra);

            if (producto_existe != null)
                return new GenericResponse<Producto> { IsSuccess = false, ErrorMessage = "El codigo de barra ya existe" };
                //throw new TaskCanceledException("El codigo de barra ya existe");
            try
            {
                entidad.NombreImagen = NombreImagen;
                if (imagen != null)
                {
                    string urlImage = await _fireBaseServicio.SubirStorageAsync(imagen, "carpeta_producto", NombreImagen);
                    entidad.UrlImagen = urlImage;

                }

                Producto producto_creado = await _repositorio.CrearAsync(entidad);

                if (producto_creado.IdProducto == 0)
                    return new GenericResponse<Producto> { IsSuccess = false, ErrorMessage = "No se pudo crear el producto" };
                //throw new TaskCanceledException("No se pudo crear el producto");

                IQueryable<Producto> query = await _repositorio.ConsultarAsync(p => p.IdProducto == producto_creado.IdProducto);

                producto_creado = query.Include(c => c.IdCategoriaNavigation).First();

                return new GenericResponse<Producto> { IsSuccess =  true, DirectObject = producto_creado };

            }
            catch (Exception exception)
            {
                return new GenericResponse<Producto> { IsSuccess = false, ErrorMessage = exception.Message };
            }
        }

        public async Task<GenericResponse<Producto>> EditarAsync(Producto entidad, Stream imagen = null, string NombreImagen = "")
        {
            Producto producto_existe = await _repositorio.ObtenerAsync(p => p.CodigoBarra == entidad.CodigoBarra && p.IdProducto != entidad.IdProducto);

            if (producto_existe != null)
                return new GenericResponse<Producto> { IsSuccess = false, ErrorMessage = "El codigo de barra ya existe" };
                //throw new TaskCanceledException("El codigo de barra ya existe");
            try
            {
                IQueryable<Producto> queryProducto = await _repositorio.ConsultarAsync(p => p.IdProducto == entidad.IdProducto);

                Producto producto_para_editar = queryProducto.First();

                producto_para_editar.CodigoBarra = entidad.CodigoBarra;
                producto_para_editar.Marca = entidad.Marca;
                producto_para_editar.Descripcion = entidad.Descripcion;
                producto_para_editar.IdCategoria = entidad.IdCategoria;
                producto_para_editar.Stock = entidad.Stock;
                producto_para_editar.Precio = entidad.Precio;
                producto_para_editar.EsActivo = entidad.EsActivo;

                if (producto_para_editar.NombreImagen == "")
                {
                    producto_para_editar.NombreImagen = NombreImagen;
                }

                if (imagen != null)
                {
                    string urlImagen = await _fireBaseServicio.SubirStorageAsync(imagen, "carpeta_producto", producto_para_editar.NombreImagen!);
                    producto_para_editar.UrlImagen = urlImagen;
                }

                bool respuesta = await _repositorio.EditarAsync(producto_para_editar);

                if (!respuesta)
                    return new GenericResponse<Producto> { IsSuccess = false, ErrorMessage = "No se pudo editar el producto" };
                //throw new TaskCanceledException("No se pudo editar el producto");


                Producto producto_editado = queryProducto.Include(c => c.IdCategoriaNavigation).First();

                return new GenericResponse<Producto> { IsSuccess = true, DirectObject = producto_editado };

            }
            catch (Exception exception)
            {
                return new GenericResponse<Producto> { IsSuccess = false, ErrorMessage = exception.Message };
            }
        }

        public async Task<GenericResponse<object>> EliminarAsync(int idProducto)
        {
            try
            {
                Producto producto_encontrado = await _repositorio.ObtenerAsync(p => p.IdProducto == idProducto);

                if (producto_encontrado == null)
                    throw new TaskCanceledException("El producto no existe");

                string nombreImagen = producto_encontrado.NombreImagen!;

                bool respuesta = await _repositorio.EliminarAsync(producto_encontrado);

                if (respuesta)
                    await _fireBaseServicio.EliminarStorageAsync("carpeta_producto", nombreImagen);

                return new GenericResponse<object> { IsSuccess = true, };
            }
            catch (Exception exception)
            {
                return new GenericResponse<object> { IsSuccess = false, ErrorMessage = exception.Message};
            }
        }

        public async Task<GenericResponse<Producto>> ListaAsync()
        {
            try
            {
                IQueryable<Producto> query = await _repositorio.ConsultarAsync();
                return new GenericResponse<Producto> { IsSuccess = true, ListObjet = query.Include(c => c.IdCategoriaNavigation).ToList() };
            }
            catch (Exception ex)
            {

                return new GenericResponse<Producto> { IsSuccess = false, ErrorMessage = ex.Message};
            }
        }
    }
}
