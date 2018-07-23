using VillasBonaterra.Models;
using VillasBonaterra.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VillasBonaterra.Controllers
{
    public class AccessController : Controller
    {
        Villas_BonaterraEntities db = new Villas_BonaterraEntities();

        // GET: Home
        public ActionResult Index()
        {
            var datos = (from subject in db.Asuntos from accesos in db.Accesos from domicilio in db.Domicilios from calle in db.Calles from visitante in db.Visitantes where subject.id_asunto==accesos.id_asunto && visitante.id_visitante==accesos.id_visitante && accesos.id_domicilio==domicilio.id_domicilio && calle.id_calle==domicilio.id_calle select new AccessViewModel { Date = accesos.fecha, DateOut = accesos.fecha_salida, Street = calle.nombre, no = domicilio.no_casa, Plate = accesos.placa,  Visitor = visitante.nombre + " " + visitante.apellido1 + " " + visitante.apellido2,Subject= subject.nombre, Idenfitication = visitante.foto  }).ToList();
            ViewBag.Current = "Access";
            return View(datos);
        }

        public ActionResult Active()
        {
            var datos = (from subject in db.Asuntos from accesos in db.Accesos from domicilio in db.Domicilios from calle in db.Calles from visitante in db.Visitantes where accesos.fecha_salida == null && subject.id_asunto == accesos.id_asunto && visitante.id_visitante == accesos.id_visitante && accesos.id_domicilio == domicilio.id_domicilio && calle.id_calle == domicilio.id_calle select new AccessViewModel { Date = accesos.fecha, DateOut= accesos.fecha_salida, Street = calle.nombre, no = domicilio.no_casa, Plate = accesos.placa, Visitor = visitante.nombre + " " + visitante.apellido1 + " " + visitante.apellido2, Subject = subject.nombre, Cone = accesos.cono ,Idenfitication = visitante.foto }).ToList();
            ViewBag.Current = "Access";
            return View(datos);
        }

    }
}