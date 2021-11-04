using CVParse2.Models;
using CVParse2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CVParse2.Controllers
{
    public class RegisterController : Controller
    {
        // GET: Register
        public ActionResult Index()
        {
            return View("Register");
        }
        public ActionResult Register(RegisterModel registerModel)
        {
            SercurityService sercurity = new SercurityService();
            if (registerModel.Password == registerModel.RePassword)
            {
                sercurity.Create(registerModel);
                ViewBag.Message = "Register successfully";
                return View("~/Views/Login/Login.cshtml");
            }
            else 
            {
                ViewBag.Message = "repeat password is not right";
                return View("Register");
            }
        }
    }
}