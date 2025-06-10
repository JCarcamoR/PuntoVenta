using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PuntoVenta.Data
{
    public class BdConnection
    {
        internal string strAmbiente = string.Empty;
        internal string strConnectionBD = string.Empty;
        internal SqlConnection connection;
        internal SqlTransaction transaction;

        // Constructor
        public BdConnection()
        {
            strAmbiente = ConfigurationManager.AppSettings["Ambiente"].ToString();
            strConnectionBD = ConfigurationManager.ConnectionStrings[(strAmbiente == "QA" ? "ConexionQA" : "ConexionPRD")].ConnectionString;
        }
        
        public string GetConnection()
        {
            return strConnectionBD;
        }
    }
}