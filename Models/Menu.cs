using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PuntoVenta.Models
{
    public class Menus : List<Menu> { }

    public class Menu
    {
        public int MEN_PK_ID { get; set; }
        public int MEN_NO_NIVEL { get; set; }
        public int MEN_ID_PADRE { get; set; }
        public string MEN_DS_MENU { get; set; }
        public string MEN_DS_ICONO { get; set; }
        public string MEN_DS_RUTA { get; set; }
        public int EDO_PK_ID { get; set; }
        public DateTime MEN_FE_UMOD { get; set; }
        public string ACC_DS_USR { get; set; }
        public int MEN_NO_ORDEN { get; set; }
        public string MEN_DS_CLASS { get; set; }
    }
}