using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VillasBonaterra.Models;

namespace VillasBonaterra.Controllers
{
    public class UsersController : Controller
    {
        Villas_BonaterraEntities db = new Villas_BonaterraEntities();
        // GET: Users
        public ActionResult Index()
        {
            var datos = (from users in db.Usuario select users).OrderBy(s => s.nombre).ThenBy(s => s.apellido1).ThenBy(s => s.apellido2).ToList();
            return View(datos);
        }
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }
    }
}