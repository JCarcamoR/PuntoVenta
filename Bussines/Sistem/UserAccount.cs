using PuntoVenta.Bussines.Services;
using PuntoVenta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace PuntoVenta.Bussines.Sistem
{
    public class UserAccount
    {
        public Error error { get; set; }

        public UserAccount() {
        }

        public void EnviarCorreoContacto(Contact contacto)
        {
            try
            {
                bool result = false;
                SendMails mail = new SendMails();
                
                result = mail.SendMailContactSupport(contacto);

                if (!result) {
                    error = mail.error;
                }
            }
            catch (Exception ex)
            {
                error.message = ex.ToString();
                error.tipoError = "Excepcion";
                error.isError = true;
            }
        }


    }
}