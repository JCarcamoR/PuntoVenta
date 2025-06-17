using PuntoVenta.Models;
using System;
using System.Configuration;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using System.Text;

namespace PuntoVenta.Bussines.Services
{
    public class SendMails
    {
        string configuracionSMTP = ConfigurationManager.AppSettings["ConfiguracionSMTP"].ToString();
        string contactSupport = string.Empty;
        string passwordSMTP = string.Empty;
        string senderSMTP = string.Empty;
        string hostSMPT = string.Empty;
        string userSMTP = string.Empty;
        public Errors error;
        int portSMPT = 0;
        bool ssl = false;

        private void GetDataMail()
        {
            if (configuracionSMTP == "1")
            {
                contactSupport = ConfigurationManager.AppSettings["ContactSupport"].ToString();
                passwordSMTP = ConfigurationManager.AppSettings["PasswordSMTP"].ToString();
                portSMPT = Convert.ToInt32(ConfigurationManager.AppSettings["PortSMTP"].ToString());
                senderSMTP = ConfigurationManager.AppSettings["SenderSMTP"].ToString();
                hostSMPT = ConfigurationManager.AppSettings["HostSMTP"].ToString();
                userSMTP = ConfigurationManager.AppSettings["UserSMTP"].ToString();
                ssl = Convert.ToBoolean(ConfigurationManager.AppSettings["SSL"]);
                error = new Errors();
            }
            else
            {

            }
        }

        public SendMails()
        {
            GetDataMail();
        }

        internal bool SendMailContactSupport(Contact contac)
        {
            try
            {
                bool response = false;
                StringBuilder cuerpo = new StringBuilder();

                cuerpo.AppendLine($"El usuario {contac.Nombre} ha levantado una nueva solicitud.");
                cuerpo.AppendLine("");
                cuerpo.AppendLine("Descripcion del evento: ");
                cuerpo.AppendLine(contac.Descripcion);
                cuerpo.AppendLine("");
                cuerpo.AppendLine("Datos de contacto...");
                cuerpo.AppendLine("");
                cuerpo.AppendLine($"Telfono: {contac.Telefono}");
                cuerpo.AppendLine($"Correo: {contac.Correo}");
           
                response = SendMail(hostSMPT, portSMPT, ssl, userSMTP, passwordSMTP, senderSMTP, contactSupport, contac.Asunto, cuerpo.ToString(), false);

                return response;
            }catch(Exception ex)
            {
                error.message = ex.ToString();
                error.isError = true;
                error.tipoError = "Excepcion";
                return false;
            }
        }


        private bool SendMail(string servidorSmtp, 
                              int puertoSmtp, 
                              bool usarSsl, 
                              string usuario, 
                              string contraseña,
                              string remitente,
                              string destinatarios, 
                              string asunto, 
                              string cuerpo, 
                              bool esHtml = true, 
                              string destinatariosCopia = null, 
                              string destinatariosCopiaOculta = null,
                              string[] archivosAdjuntos = null)
        {
            try
            {
                // Crear el mensaje de correo
                var mensaje = new MailMessage
                {
                    From = new MailAddress(remitente),
                    Subject = asunto ?? string.Empty,
                    Body = cuerpo ?? string.Empty,
                    IsBodyHtml = esHtml
                };

                // Agregar destinatarios principales
                foreach (var email in destinatarios.Split(new[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    mensaje.To.Add(email.Trim());
                }

                // Agregar CC si se especificó
                if (!string.IsNullOrWhiteSpace(destinatariosCopia))
                {
                    foreach (var email in destinatariosCopia.Split(new[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        mensaje.CC.Add(email.Trim());
                    }
                }

                // Agregar BCC si se especificó
                if (!string.IsNullOrWhiteSpace(destinatariosCopiaOculta))
                {
                    foreach (var email in destinatariosCopiaOculta.Split(new[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        mensaje.Bcc.Add(email.Trim());
                    }
                }

                // Adjuntar archivos si se especificaron
                if (archivosAdjuntos != null && archivosAdjuntos.Length > 0)
                {
                    foreach (var rutaArchivo in archivosAdjuntos)
                    {
                        if (!string.IsNullOrWhiteSpace(rutaArchivo) && System.IO.File.Exists(rutaArchivo))
                        {
                            var adjunto = new Attachment(rutaArchivo, MediaTypeNames.Application.Octet);
                            mensaje.Attachments.Add(adjunto);
                        }
                    }
                }

                // Configurar el cliente SMTP
                var clienteSmtp = new SmtpClient(servidorSmtp, puertoSmtp)
                {
                    EnableSsl = usarSsl,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(usuario, contraseña)
                };

                // Enviar el correo
                clienteSmtp.Send(mensaje);

                // Liberar recursos de los adjuntos
                foreach (var adjunto in mensaje.Attachments)
                {
                    adjunto.Dispose();
                }

                return true;
            }
            catch (Exception ex)
            {
                error.message = ex.ToString();
                error.isError = true;
                error.tipoError = "Excepcion";
                return false;
            }
        }
    }
}