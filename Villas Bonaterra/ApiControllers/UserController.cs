using VillasBonaterra.Help;
using VillasBonaterra.Models;
using VillasBonaterra.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Security;
using System.Web;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace VillasBonaterra.ApiControllers
{
    public class UserController : ApiController
    {
        Villas_BonaterraEntities db = new Villas_BonaterraEntities();

        [AllowAnonymous]
        [ActionName("Authenticate")]
        [HttpGet]
        public HttpResponseMessage Authenticate([FromUri] UserLoginAPPViewModel model)
        {
            var json = "";

            if (!String.IsNullOrEmpty(model.user) || !String.IsNullOrEmpty(model.password))
            {

                //var user = (from us in db.Usuario where us.nombre.ToLower() == model.user.ToLower() && us.password ==  model.password select ac).FirstOrDefault();

                if (Membership.ValidateUser(model.user, model.password))
                {
                    if (Helper.IsInRole(model.user, "administrator"))
                    {
                        json = Helper.toJson(true, "OK");

                        return new HttpResponseMessage()
                        {
                            Content = new StringContent(json.ToString(), Encoding.UTF8, "application/json")
                        };
                    }
                    else {
                        json = Helper.toJson(false, "Usuario sin Privilegios");

                        return new HttpResponseMessage()
                        {
                            Content = new StringContent(json.ToString(), Encoding.UTF8, "application/json")
                        };
                    }
                    
                }
                else {
                    json = Helper.toJson(false, "Acceso denegado");

                    return new HttpResponseMessage()
                    {
                        Content = new StringContent(json.ToString(), Encoding.UTF8, "application/json")
                    };
                }
            }

            json = Helper.toJson(false, "Falta parametros");

            return new HttpResponseMessage()
            {
                Content = new StringContent(json.ToString(), Encoding.UTF8, "application/json")
            };
        }

        [AllowAnonymous]
        [ActionName("Add")]
        [HttpPost]
        public HttpResponseMessage AddUser([FromBody] UserAddViewModel model)
        {
            Usuario user = new Usuario();
            var json = "";
            try
            {
                user.id = Guid.NewGuid();
                user.nombre = model.Name;
                user.apellido1 = model.MidleName;
                user.apellido2 = model.LastName;
                user.telefono = model.Telefono;
                user.email = model.Correo;

                db.Usuario.Add(user);
                db.SaveChanges();

                json = Helper.toJson(true, "Usuario Agregado");
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

                    json = Helper.toJson(false, "No se pudo agregar usuario");

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
