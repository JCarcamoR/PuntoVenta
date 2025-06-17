using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using PuntoVenta.Bussines.Sistem;
using PuntoVenta.Data;
using PuntoVenta.Models;

namespace PuntoVenta.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
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
                Errors error = new Errors();

                error = security.ValidaAccesos(user.ACC_DS_USR, user.ACC_DS_PWDR);

                if (error != null && !error.isError) 
                {
                    // Opción 1: Usando FormsAuthentication
                    FormsAuthentication.SetAuthCookie(user.ACC_DS_USR, user.session.Rememberme);

                    // Opción 2: Creando manualmente el ticket (más control)
                    var authTicket = new FormsAuthenticationTicket(
                        1,                             // versión
                        user.ACC_DS_USR,                // nombre de usuario
                        DateTime.Now,                  // creación
                        DateTime.Now.AddMinutes(30),    // expiración
                        user.session.Rememberme,               // persistente?
                        ""                             // datos de usuario (roles, etc)
                    );

                    string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                    var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    if (user.session.Rememberme)
                    {
                        authCookie.Expires = authTicket.Expiration;
                    }
                    Response.Cookies.Add(authCookie);

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

        // POST: Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
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