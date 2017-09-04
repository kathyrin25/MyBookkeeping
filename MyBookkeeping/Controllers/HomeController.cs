using MyBookkeeping.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MyBookkeeping.Filters;

namespace MyBookkeeping.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //直接導到作業
            return RedirectToAction("AddRecord", "Bookkeeping", new { Page = 1 });
            //return View();
        }

        [CheckAuthorizeActionFilter(AuthRole = "Admin")]
        public ActionResult About()
        {
            ViewBag.Message = "About page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact page.";

            return View();
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(LoginViewModel login)
        {            
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            //這個先暫時寫hard code代替
            var role = String.Empty;
            if (login.Account == "aa@aa.com")
            {
                role = "Admin";
            }
            else if (login.Account == "bb@bb.com")
            {
                role = "Normal";
            }

            //驗證通過
            var ticket = new FormsAuthenticationTicket(
                version: 1,
                name: login.Account,
                issueDate: DateTime.Now,
                expiration: DateTime.Now.AddMinutes(30),
                isPersistent: false,
                userData: role,
                cookiePath: FormsAuthentication.FormsCookiePath);

            // Encrypt the ticket.
            string encTicket = FormsAuthentication.Encrypt(ticket);

            // Create the cookie.
            Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
                        
            return Redirect(FormsAuthentication.GetRedirectUrl(login.Account, false));
        }



        [AllowAnonymous]
        public ActionResult Logout()
        {
            Response.Cookies.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Home");
        }
    }
}