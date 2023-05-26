
using System.Net.Mail;

namespace NetMarketGestor.Utilidades
{
    public class Correos
    {
        public void EnviarCorreo(string destinatario, string asunto, string mensaje)
        {
            var correoTienda = "99diazluisfernand@gmail.com";
            var password = "edliuzcxpjttcfky";

            MailMessage mailMessage = new MailMessage(correoTienda, destinatario, asunto, mensaje);

            mailMessage.IsBodyHtml = true;

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Port = 587;
            smtpClient.Credentials = new System.Net.NetworkCredential(correoTienda, password);

            smtpClient.Send(mailMessage);

            smtpClient.Dispose();
        }
    }

}
