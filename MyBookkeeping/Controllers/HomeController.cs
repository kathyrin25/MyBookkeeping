using MyBookkeeping.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

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

            FormsAuthentication.RedirectFromLoginPage(login.Account, false);
            
            return Redirect(FormsAuthentication.GetRedirectUrl(login.Account, false));
        }

        [AllowAnonymous]
        public ActionResult Logout()
        {  
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Home");
        }
    }
}