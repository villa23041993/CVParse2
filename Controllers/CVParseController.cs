using CVParse2.Models;
using CVParse2.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CVParse2.Controllers
{
    public class CVParseController : Controller
    {
        // GET: CVParse
        public ActionResult Index()
        {
            return View("CVParse1");
        }
        
        public ActionResult Parsing(Test data)
        {
            /*ViewBag.Message = data.Description;
            return View("CVParse1");*/
            ProfileDAO profileDAO = new ProfileDAO();
             ProfilModel profile = profileDAO.Extract(data);
            

            return View("Parsing",profile);
        }
    }
}