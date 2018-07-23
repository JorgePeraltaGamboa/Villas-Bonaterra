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
    }
}
