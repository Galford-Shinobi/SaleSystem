namespace SaleSystem.BLL.Interfaces
{
    public interface IFireBaseService
    {

        Task<string> SubirStorageAsync(Stream StreamArchivo, string CarpetaDestino, string NombreArchivo);
        Task<bool> EliminarStorageAsync(string CarpetaDestino, string NombreArchivo);

    }
}
