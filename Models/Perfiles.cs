using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PuntoVenta.Models
{
    public class Perfiles : List<Perfil> {}

    public class Perfil
    {
        public int PER_PK_ID            {get; set;}
        public string PER_DS_PERFIL    {get; set;}
        public int EDO_PK_ID            {get; set;}
        public DateTime PER_FE_UMOD     {get; set;}
        public string ACC_DS_USR { get; set; }
    }
}