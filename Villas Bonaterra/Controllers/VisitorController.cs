using VillasBonaterra.Models;
using VillasBonaterra.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace VillasBonaterra.Controllers
{
    public class VisitorController : Controller
    {
        Villas_BonaterraEntities db = new Villas_BonaterraEntities();
        // GET: Visitor
        public ActionResult Index()
        {
            var datos = (from visitor in db.Visitantes select visitor).OrderBy(s => s.nombre).ThenBy(s => s.apellido1).ThenBy(s => s.apellido2).ToList();
            return View(datos);
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Update(Guid id)
        {
            var Visitor = db.Visitantes.Where(x => x.id_visitante == id).FirstOrDefault();
            ViewBag.CheckedBy = Visitor.VerificacionVisitantes.Select(w => w.Usuario.nombre+ " "+ w.Usuario.apellido1 + " " + w.Usuario.apellido2).FirstOrDefault();
            return View(Visitor);
        }
    }
}