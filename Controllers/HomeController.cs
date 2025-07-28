using PuntoVenta.Bussines.Sistem;
using PuntoVenta.Data;
using PuntoVenta.Models;
using System;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PuntoVenta.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            if (Session["UserAutenticate"] != null)
            {
                if (Session["UserAutenticate"] == "false")
                {
                    return RedirectToAction("LogIn", "Home");
                }
            }

            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            Response.Cache.SetExpires(DateTime.UtcNow.AddSeconds(-1));

            Menus ltsMenus = new Menus();
            Security security = new Security();

            ltsMenus = security.GetMenuPerfil("ADMINISTRADOR");

            if (security.errors.Count > 0)
            {
                StringBuilder error = new StringBuilder();
                int cnt = 1;
                foreach (Error e in security.errors)
                {
                    if (e.isError)
                    {
                        cnt++;
                        error.AppendLine($"Error {cnt}: Tipo de Error : {e.tipoError} - Descripcion: {e.message}");
                    }
                }
                ViewBag.error = error.ToString();
            }

            return View(ltsMenus);
        }

        [AllowAnonymous]
        public ActionResult LogIn(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Accesos user, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                Security security = new Security();
                Error error = new Error();

                error = security.ValidaAccesos(user.ACC_DS_USR, user.ACC_DS_PWDR);

                if (error != null && !error.isError)
                {
                    // Solo esta línea es suficiente:
                    FormsAuthentication.SetAuthCookie(user.ACC_DS_USR, user.session.Rememberme);

                    Session["UserAutenticate"] = "true";

                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Usuario o contraseña incorrectos.");
                }
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();
            Session["UserAutenticate"] = "false";
            return RedirectToAction("LogIn", "Home");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [HttpPost]
        public JsonResult Contact(Contact contacto)
        {
            UserAccount user = new UserAccount();
            user.EnviarCorreoContacto(contacto);

            if (user.error != null)
            {
                return Json(new { success = false, message = user.error.message });
            }
            else
            {
                return Json(new { success = true, message = "Solicitud de contacto enviada correctamente."});
            }
        }
    }
}