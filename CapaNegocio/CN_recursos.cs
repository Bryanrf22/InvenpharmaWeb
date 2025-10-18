using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace CapaNegocio
{
    public class CN_recursos
    {
        public static string GenerarClave()
        {
            string clave = Guid.NewGuid().ToString("N").Substring(0, 8);
            return clave;
        }

        public static string ConvertirSHA256(string texto)
        {
            //using System.Security.Cryptography;
            StringBuilder sb = new StringBuilder();
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(texto));

                foreach (byte b in result)
                    sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }

        public static bool EnviarCorreo(string correo, string asunto, string mensaje)
        {
            bool resultado = false;
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(correo);
                mail.From = new MailAddress("bryanromerof26@gmail.com");
                mail.Subject = asunto;
                mail.Body = mensaje;
                mail.IsBodyHtml = true;

                var smtp = new SmtpClient()
                {
                    Credentials = new NetworkCredential("bryanromerof26@gmail.com", "nkfy wone cxtl hxmp"),
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true
                };

                smtp.Send(mail);
                resultado = true;
            }
            catch (Exception ex)
            {
                resultado = false;
            }

            return resultado;
        }

        public static string ConvertirBase64(string ruta, out bool conversion)
        {
            string textoBase64 = string.Empty;
            conversion = true;
            try
            {
                byte[] bytes = File.ReadAllBytes(ruta);
                textoBase64 = Convert.ToBase64String(bytes);
                
            }
            catch (Exception ex)
            {
                textoBase64 = string.Empty;
                conversion = false;
            }
            return textoBase64;
        }

    }
}