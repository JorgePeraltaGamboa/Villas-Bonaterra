using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using VillasBonaterra.Models;

namespace VillasBonaterra.ApiControllers
{
    public class CardController : ApiController
    {
        Villas_BonaterraEntities db = new Villas_BonaterraEntities();

        [AllowAnonymous]
        [HttpGet]
        [ActionName("all")]
        public HttpResponseMessage GetSubjects(string id = "")
        {

            List<String> cards = new List<String>();


            cards = (from card in db.TarjetasAcceso where card.Activa == true select card.Numero).ToList();

            /*
            return new HttpResponseMessage()
            {
                Content = new StringContent(JArray.FromObject(cards).ToString(), Encoding.UTF8, "application/json")
            };
            */

            return new HttpResponseMessage()
            {
                Content = new StringContent(string.Join("\n", cards), System.Text.Encoding.UTF8, "text/plain")
        };
        }
    }
}
