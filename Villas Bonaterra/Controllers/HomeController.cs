using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VillasBonaterra.Models;
using VillasBonaterra.ViewModels;

namespace VillasBonaterra.Controllers
{
    [Authorize(Roles = "administrator,user")]
    public class HomeController : Controller
    {
        Villas_BonaterraEntities db = new Villas_BonaterraEntities();

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
    }
}