using PuntoVenta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PuntoVenta.Bussines.Sistem
{
    public class UserAccount
    {
        public string error { get; set; }

        public UserAccount() {
            error = "";
        }
        public void EnviarCorreoContacto(Contact contacto)
        {
            try
            {

            }
            catch (Exception ex)
            {
                error = ex.ToString();
            }
        }


    }
}