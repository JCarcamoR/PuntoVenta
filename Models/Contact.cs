using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PuntoVenta.Models
{
    public class Contact
    {
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string Asunto { get; set; }
        public string Descripcion { get; set; }
    }
}