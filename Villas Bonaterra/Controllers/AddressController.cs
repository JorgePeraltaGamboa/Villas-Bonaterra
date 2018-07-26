using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VillasBonaterra.Models;
using VillasBonaterra.ViewModels;

namespace VillasBonaterra.Controllers
{
    public class AddressController : Controller
    {
        // GET: Address
        Villas_BonaterraEntities db = new Villas_BonaterraEntities();
        public ActionResult Index(Guid id)
        {
          
            var user = db.Usuario.Where(x => x.id == id).FirstOrDefault();
            List <Domicilios> Address = user.Domicilios.ToList();

            AddressUsersViewModel model = new AddressUsersViewModel();
            model.Owner = user.nombre + "";
            model.Owner_Id = user.id;
            model.Address = Address;
            model.Streets = db.Calles.ToList();
            model.Owners = db.Usuario.ToList();

            return View(model); 
        }
    }
}