using SaleSystem.Common.Response;

namespace SaleSystem.BLL.Interfaces
{
    public interface ICorreoService
    {
        GenericResponse<object> SendMail(string to, string subject, string body);
        Task<GenericResponse<object>> EnviarCorreoAsync(string to, string subject, string body);
    }
}
