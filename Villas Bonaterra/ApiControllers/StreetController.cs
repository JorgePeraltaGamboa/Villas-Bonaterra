using Newtonsoft.Json.Linq;
using VillasBonaterra.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace VillasBonaterra.ApiControllers
{
    public class StreetController : ApiController
    {
        Villas_BonaterraEntities db = new Villas_BonaterraEntities();

        [AllowAnonymous]
        [HttpGet]
        [ActionName("all")]
        public HttpResponseMessage GetSubjects(string id = "")
        {

            List<Street> streets = new List<Street>();


            streets = (from street in db.Calles orderby street.nombre ascending select new Street { Id = street.id_calle, Name = street.nombre }).ToList();


            return new HttpResponseMessage()
            {
                Content = new StringContent(JArray.FromObject(streets).ToString(), Encoding.UTF8, "application/json")
            };
        }

        public class Street
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }
    }
}
