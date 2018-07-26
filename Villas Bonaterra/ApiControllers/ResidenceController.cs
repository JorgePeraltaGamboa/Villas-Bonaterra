using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using VillasBonaterra.Help;
using VillasBonaterra.Models;
using VillasBonaterra.ViewModels;

namespace VillasBonaterra.ApiControllers
{
    public class ResidenceController : ApiController
    {
        Villas_BonaterraEntities db = new Villas_BonaterraEntities();
        [AllowAnonymous]
        [ActionName("Add")]
        [HttpPost]
        public HttpResponseMessage AddResidence([FromBody] ResidencenceAddViewModel model)
        {
            Domicilios residence = new Domicilios();
            var json = "";
            try
            {
                residence.id_domicilio = Guid.NewGuid();
                residence.no_casa = model.No_Calle;
                residence.id_usuario = model.OwnerId;
                residence.id_calle = model.Street;

                db.Domicilios.Add(residence);
                db.SaveChanges();

                json = Helper.toJson(true, "Domicilio Agregado");
                return new HttpResponseMessage()
                {
                    Content = new StringContent(json.ToString(), Encoding.UTF8, "application/json")
                };
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                    eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        //Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",ve.PropertyName, ve.ErrorMessage);
                        EventLog eventLog = new EventLog("Application");
                        eventLog.Source = "BonaterraSite";
                        eventLog.WriteEntry("- Property: " + ve.PropertyName + ", Error: " + ve.ErrorMessage, EventLogEntryType.Information, 101, 1);

                    }

                    json = Helper.toJson(false, "No se pudo agregar el nuevo domicilio");

                    return new HttpResponseMessage()
                    {
                        Content = new StringContent(json.ToString(), Encoding.UTF8, "application/json")
                    };
                }
                throw;
            }
        }
    }
}
