using PuntoVenta.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace PuntoVenta.Data
{
    public class Security
    {
        BdConnection bd = null; 
        string connectionString = String.Empty;

        public Security()
        {
            bd = new BdConnection();
            connectionString = bd.GetConnection();
        }

        public Errors ValidaAccesos(string usuario, string password)
        {
            Errors errors = new Errors();
            try
            {
                Accesos user = new Accesos();
                user = ObtenUsuario(usuario);

                if (user != null) 
                {
                    switch (user.EDO_PK_ID) // Se verifica el estado del usuario
                    {
                        case 2: // Activo
                            errors.isError = false;
                            break;
                        case 4: // Inactivo
                            errors.isError = true;
                            errors.message = "El usuario ***" + usuario + "*** se encunetra INACTIVO.";
                            break;
                        case 6: // Bloqueado
                            errors.isError = true;
                            errors.message = "Usuario ***" + usuario + "*** BLOQUEADO.";
                            break;
                    }

                    if (user.ACC_DS_PWDR == password)
                    {
                        errors.isError = false;
                    }
                    else
                    {
                        errors.isError = true;
                        errors.tipoError = "DATOS";
                        errors.message = "Usuario o Contraseña INCORRECTOS";
                    }
                    return errors;
                }
                else
                {
                    errors.isError = true;
                    errors.message = "Usuario ***" + usuario + "*** BLOQUEADO.";
                    return errors;
                }
            }
            catch (Exception ex)
            {
                errors.isError = true;
                errors.tipoError = "EXCEPCION";
                errors.message = ex.ToString();
                return errors;
            }
        }

        private Accesos ObtenUsuario(string strUsuario)
        {
            Accesos Usuario = new Accesos();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT * FROM ACCESOS WHERE ACC_DS_USR = @Usuario";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Usuario", strUsuario);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        if (reader.HasRows)
                        {
                            Usuario.ACC_PK_ID = Int32.Parse(reader[0].ToString());
                            Usuario.ACC_DS_USR = reader[1].ToString();
                            Usuario.ACC_DS_PWDR = reader[2].ToString();
                            Usuario.ACC_FE_UACC = DateTime.Parse(reader[3].ToString());
                            Usuario.EDO_PK_ID   = Int32.Parse(reader[4].ToString());
                            Usuario.ACC_FE_UMOD = DateTime.Parse(reader[5].ToString());
                            Usuario.ACC_DS_USRM = reader[6].ToString();
                        }
                    }
                }
                return Usuario;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private void If(object value)
        {
            throw new NotImplementedException();
        }
    }
}