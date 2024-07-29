using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DokumanModulu.Entity;
using DokumanModulu.Identity;
using DokumanModulu.Models;

namespace DokumanModulu.Controllers
{
    public class HomeController : Controller
    {
        IdentityDataContext db = new IdentityDataContext();

        
        public ActionResult Index()
        {
            return View();
        }

        



       
        

        



    }
}
