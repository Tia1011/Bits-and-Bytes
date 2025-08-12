using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tiani.P_Bites_Bytes.Models;

namespace Tiani.P_Bites_Bytes.Controllers
{
    public class HomeController : Controller
    {

        //create an instance of the database context
        private BitsAndBytesDbContext context = new BitsAndBytesDbContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

       
    }
}