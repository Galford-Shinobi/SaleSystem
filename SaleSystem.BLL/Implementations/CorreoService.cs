using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using SaleSystem.BLL.Interfaces;
using SaleSystem.Common.Response;

namespace SaleSystem.BLL.Implementations
{
    public class CorreoService : ICorreoService
    {
        private readonly IConfiguration _configuration;

        public CorreoService(IConfiguration Configuration)
        {
            _configuration = Configuration;
        }

        public GenericResponse<object> SendMail(string to, string subject, string body)
        {
            try
            {
                string DisplayName = _configuration["Mail:DisplayName"];
                string from = _configuration["Mail:From"];
                string smtp = _configuration["Mail:Smtp"];
                string port = _configuration["Mail:Port"];
                string password = _configuration["Mail:Password"];

                MimeMessage message = new MimeMessage();
                message.From.Add(new MailboxAddress(DisplayName, from));
                message.To.Add(new MailboxAddress("Master", to));
                message.Subject = subject;
                BodyBuilder bodyBuilder = new BodyBuilder
                {
                    HtmlBody = body
                };
                message.Body = bodyBuilder.ToMessageBody();

                using (SmtpClient client = new SmtpClient())
                {
                    client.Connect(smtp, int.Parse(port), false);
                    client.Authenticate(from, password);
                    client.Send(message);
                    client.Disconnect(true);
                }

                return new GenericResponse<object> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                return new GenericResponse<object>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message,
                    DirectObject = ex
                };
            }
        }

    }
}
