using VillasBonaterra.Models;
using VillasBonaterra.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static VillasBonaterra.ApiControllers.StreetController;

namespace VillasBonaterra.Controllers
{
    public class StreetController : Controller
    {
        Villas_BonaterraEntities db = new Villas_BonaterraEntities();
        // GET: Street
        public ActionResult Index()
        {
            List<StreetViewModel> streets = new List<StreetViewModel>();
            List<StreetViewModel> streetsView = new List<StreetViewModel>();

            streets = (from street in db.Calles from address in db.Domicilios where address.id_calle==street.id_calle orderby street.nombre ascending select new StreetViewModel { Id = street.id_calle, Name = street.nombre, Numbers = address.no_casa }).ToList();

            //var result = streets.GroupBy(cc => cc.Name).Select(dd => new { Name = dd.Key, Id = dd.Select(w => w.Id).FirstOrDefault(), Numbers = string.Join(",", dd.Select(ee => ee.Numbers).ToList())}).ToList();
              var result = streets.GroupBy(cc => cc.Name).Select(dd => new { Name = dd.Key, Id = dd.Select(w => w.Id).FirstOrDefault(), Numbers = string.Join(",", dd.Select(ee => ee.Numbers).OrderBy(g => g).ToList())}).ToList();
            streetsView = result.Select(s => new StreetViewModel { Id = s.Id, Name = s.Name, Numbers = s.Numbers }).ToList();
            
            return View(streetsView);
        }
    }
}