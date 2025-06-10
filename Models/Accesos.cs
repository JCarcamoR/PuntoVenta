using PuntoVenta.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PuntoVenta.Models
{
    public class Accesos
    {
        public Accesos() { 
            session = new Session();
        }    
        public int ACC_PK_ID {get; set;}
        public string ACC_DS_USR {get; set;}
        public string ACC_DS_PWDR {get; set;}
        public DateTime ACC_FE_UACC {get; set;}
        public int EDO_PK_ID {get; set;}
        public DateTime ACC_FE_UMOD {get; set;}
        public string ACC_DS_USRM { get; set; }

        public Session session {get; set;}
    }
}