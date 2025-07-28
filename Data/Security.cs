using Newtonsoft.Json.Linq;
using PuntoVenta.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Security;

namespace PuntoVenta.Data
{
    public class Security
    {
        #region "Variables privadas"
        BdConnection bd = null;
        string connectionString = String.Empty;
        Error error = null;
        #endregion

        #region "Variables publicas"
        public Errors errors = new Errors();
        #endregion

        #region "Constructores"
        public Security()
        {
            bd = new BdConnection();
            connectionString = bd.GetConnection();
        }
        #endregion

        #region "Metodos publicos"
        public Error ValidaAccesos(string usuario, string password)
        {
            Error error = new Error();
            try
            {
                Accesos user = new Accesos();
                user = ObtenUsuario(usuario);

                if (user != null)
                {
                    switch (user.EDO_PK_ID) // Se verifica el estado del usuario
                    {
                        case 2: // Activo
                            error.isError = false;
                            break;
                        case 4: // Inactivo
                            error.isError = true;
                            error.message = "El usuario ***" + usuario + "*** se encunetra INACTIVO.";
                            break;
                        case 6: // Bloqueado
                            error.isError = true;
                            error.message = "Usuario ***" + usuario + "*** BLOQUEADO.";
                            break;
                    }

                    if (user.ACC_DS_PWDR == password)
                    {
                        error.isError = false;
                    }
                    else
                    {
                        error.isError = true;
                        error.tipoError = "DATOS";
                        error.message = "Usuario o Contraseña INCORRECTOS";
                    }
                    return error;
                }
                else
                {
                    error.isError = true;
                    error.message = "Usuario ***" + usuario + "*** BLOQUEADO.";
                    return error;
                }
            }
            catch (Exception ex)
            {
                error.isError = true;
                error.tipoError = "EXCEPCION";
                error.message = ex.ToString();
                return error;
            }
        }

        public Menus GetMenuPerfil(string PER_DS_PERFIL)
        {
            if (PER_DS_PERFIL == "")
            {
                error = new Error();
                error.isError = true;
                error.tipoError = "Parametros incompletos";
                error.message = "No se localizo la informacion del perfil.";
                errors.Add(error);
                return null;
            }
            else
            {
                Menus ltsMenu = new Menus();
                ltsMenu = GetMenuPrincipal(PER_DS_PERFIL);
                return ltsMenu;
            }
        }
        #endregion

        #region "Metodos privados"

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
                            Usuario.EDO_PK_ID = Int32.Parse(reader[4].ToString());
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

        private Menus GetMenuPrincipal(string PER_DS_PERFIL)
        {
            try
            {
                Menus ltsMenus = new Menus();
                Menu menu = null;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "EXEC spSel_01_Menu @PER_DS_PERFIL";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@PER_DS_PERFIL", PER_DS_PERFIL);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        if (reader.HasRows)
                        {
                            menu = new Menu();
                            menu.MEN_PK_ID = Int32.Parse(reader[0].ToString());
                            menu.MEN_DS_MENU = reader[1].ToString();
                            menu.MEN_DS_ICONO = reader[2].ToString();
                            menu.MEN_DS_RUTA = reader[3].ToString();
                            menu.MEN_DS_CLASS = reader[4].ToString();
                            ltsMenus.Add(menu);
                        }
                    }
                }

                if (ltsMenus != null && ltsMenus.Count == 0)
                {
                    error = new Error();
                    error.isError = true;
                    error.tipoError = "ERROR EN LA INFORMACION";
                    error.message = $"No se han configurado los menus para el perfil: {PER_DS_PERFIL}.";
                    errors.Add(error);
                }

                return ltsMenus;
            }
            catch (Exception ex)
            {
                error = new Error();
                error.isError = true;
                error.message = ex.ToString();
                error.tipoError = "Excepcion";
                errors.Add(error);
                return null;
            }
        }
        #endregion
    }
}