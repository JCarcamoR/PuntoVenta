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
        public void EnviarCorreoContacto(string nombre, string telefono, string correo, string asunto, string descripcion)
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