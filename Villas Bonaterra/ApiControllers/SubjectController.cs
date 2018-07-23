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
    public class SubjectController : ApiController
    {
        Villas_BonaterraEntities db = new Villas_BonaterraEntities();

        [AllowAnonymous]
        [HttpGet]
        [ActionName("all")]
        public HttpResponseMessage GetSubjects(string id = "")
        {

            List<Subject> subjects = new List<Subject>();


            subjects = (from subject in db.Asuntos select new Subject { Id = subject.id_asunto, Name = subject.nombre }).ToList();
            
            
            return new HttpResponseMessage()
            {
                Content = new StringContent(JArray.FromObject(subjects).ToString(), Encoding.UTF8, "application/json")
            };
        }

        public class Subject
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }
    }
}
