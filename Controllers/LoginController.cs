using CVParse2.Models;
using CVParse2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CVParse2.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View("Login");
        }
        public ActionResult Login(UserModel userModel)
        {
            SercurityService sercurity = new SercurityService();
            if (sercurity.Authenticate(userModel))
                return View("~/Views/CVParse/CVParse1.cshtml");
            else
            {
                ViewBag.Message = "Login Failed";
                return View("Login");
            }
        }
    }
}