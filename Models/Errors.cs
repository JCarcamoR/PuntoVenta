using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PuntoVenta.Models
{
    public class Errors : List<Error> { }

    public class Error
    {
        public bool isError { get; set; }
        public string message { get; set; }
        public string tipoError { get; set; }
    }
}