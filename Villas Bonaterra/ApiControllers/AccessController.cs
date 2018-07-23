using Newtonsoft.Json.Linq;
using VillasBonaterra.Help;
using VillasBonaterra.Models;
using VillasBonaterra.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace VillasBonaterra.ApiControllers
{
    public class AccessController : ApiController
    {
        Villas_BonaterraEntities db = new Villas_BonaterraEntities();

        [AllowAnonymous]
        [ActionName("Add")]
        [HttpPost]
        public async Task<HttpResponseMessage> Add()
        {
            var folderName = "Content/Photos";
            var CurrentTime = Helper.Now();
            string json = "";
            var fromAndroid = false;

            var PATH = HttpContext.Current.Server.MapPath("~/" + folderName);
            var rootUrl = Request.RequestUri.AbsoluteUri.Replace(Request.RequestUri.AbsolutePath, String.Empty);
            var streamProvider = new MultipartFormDataStreamProvider(PATH);


            if (Request.Content.IsMimeMultipartContent())
            {
                await Request.Content.ReadAsMultipartAsync(streamProvider);

                var PIN = streamProvider.FormData["Pin"];
                var Plate = streamProvider.FormData["Plate"];
                var Cone = streamProvider.FormData["Cone"];
                var IdStreet = Guid.Parse(streamProvider.FormData["IdStreet"]);
                var NumberHouse = streamProvider.FormData["NumberHouse"];
                var IdSubject = Guid.Parse(streamProvider.FormData["IdSubject"]);
                var Photo = "";

                Visitantes newVisitor = (from us in db.Visitantes where us.pin == PIN select us).FirstOrDefault();
                Calles street = (from ca in db.Calles where ca.id_calle == IdStreet select ca).FirstOrDefault();
                Asuntos subject = (from sub in db.Asuntos where sub.id_asunto == IdSubject select sub).FirstOrDefault();

                Domicilios address = (from dom in db.Domicilios where dom.id_calle == IdStreet && dom.no_casa==NumberHouse select dom).FirstOrDefault();

                if (newVisitor == null)
                {

                    json = Helper.toJson(false, "No existe el visitante");

                    return new HttpResponseMessage()
                    {
                        Content = new StringContent(json.ToString(), Encoding.UTF8, "application/json")
                    };
                }

                if (address== null) {

                    json = Helper.toJson(false, "No existe el domicilio");

                    return new HttpResponseMessage()
                    {
                        Content = new StringContent(json.ToString(), Encoding.UTF8, "application/json")
                    };
                }

                foreach (MultipartFileData fileData in streamProvider.FileData)
                {
                    if (string.IsNullOrEmpty(fileData.Headers.ContentDisposition.FileName))
                    {
                        return Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted");
                    }
                    string fileName = fileData.Headers.ContentDisposition.FileName;
                    if (fileName.StartsWith("\"") && fileName.EndsWith("\""))
                    {
                        fileName = fileName.Trim('"');
                    }
                    if (fileName.Contains(@"/") || fileName.Contains(@"\"))
                    {
                        fileName = Path.GetFileName(fileName);
                    }

                    string ext = Path.GetExtension(fileName);
                    
                    var phototemp = Guid.NewGuid().ToString().ToUpper() + ext;
                    var photo = Guid.NewGuid().ToString().ToUpper() + ext;
                    var lastphoto = Guid.NewGuid().ToString().ToUpper() + ext;

                    File.Move(fileData.LocalFileName, Path.Combine(PATH, phototemp));

                    string text = System.IO.File.ReadAllText(Path.Combine(PATH, phototemp));

                    string base64 = text.Split(',')[1];
                    if (Helper.IsBase64(base64))
                    {
                        fromAndroid = true;
                        byte[] bytes = Convert.FromBase64String(base64);
                        using (Image image = Image.FromStream(new MemoryStream(bytes)))
                        {
                            image.Save(Path.Combine(PATH, photo), ImageFormat.Jpeg);  // Or Png
                        }
                    }
                    else {
                        File.Move(fileData.LocalFileName, Path.Combine(PATH, photo));
                    }
                    // First load the image somehow
                    Image myImage = Image.FromFile(Path.Combine(PATH, photo), true);

                    if (myImage.Width < myImage.Height)
                    {
                        Helper.RotateAndSaveImage(Path.Combine(PATH, photo), Path.Combine(PATH, lastphoto));
                    }
                    else {
                        myImage.Save(Path.Combine(PATH, lastphoto), System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                    // Save the image with a quality of 50%
                    myImage.Dispose();

                    Image LastImage = Image.FromFile(Path.Combine(PATH, lastphoto), true);
                    Helper.SaveJpeg(Path.Combine(PATH, "min" + lastphoto), LastImage, 40);
                    LastImage.Dispose();

                    if (File.Exists(Path.Combine(PATH, phototemp)))
                    {
                        File.Delete(Path.Combine(PATH, phototemp));
                    }
                    if (File.Exists(Path.Combine(PATH, photo)))
                    {
                        File.Delete(Path.Combine(PATH, photo));
                    }
                    Photo = lastphoto;
                }

                try
                {
                    Accesos acceso = new Accesos();
                    acceso.id_acceso = Guid.NewGuid();
                    acceso.id_visitante = newVisitor.id_visitante;
                    acceso.id_asunto = IdSubject;
                    acceso.id_domicilio = address.id_domicilio;
                    acceso.fecha = Helper.Now();
                    acceso.placa = Plate == "undefined" ? "" : Plate;
                    acceso.cono =   Cone == "undefined" ? "" : Cone;
                    acceso.identificacion = Photo;

                    db.Accesos.Add(acceso);

                    db.SaveChanges();

                    if (fromAndroid)
                    {
                        json = Helper.toJson(true, "Acceso Grabado");
                    }
                    else
                    {
                        json = Helper.toJson(true, "Acceso Grabado");
                    }


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

                        json = Helper.toJson(false, "No se pudo guardar el acceso");

                        return new HttpResponseMessage()
                        {
                            Content = new StringContent(json.ToString(), Encoding.UTF8, "application/json")
                        };
                    }
                    throw;
                }
            }
            json = Helper.toJson(false, "Error");

            return new HttpResponseMessage()
            {
                Content = new StringContent(json.ToString(), Encoding.UTF8, "application/json")
            };
        }

        [AllowAnonymous]
        [ActionName("Update")]
        [HttpGet]
        public HttpResponseMessage Update(string id = "")
        {
            var json = "";

            if (!String.IsNullOrEmpty(id))
            {
                Guid Id = Guid.Parse(id);
                var access = (from ac in db.Accesos where ac.id_acceso == Id select ac).FirstOrDefault();

                if (access != null)
                {
                    access.fecha_salida = Helper.Now();
                    db.SaveChanges();
                    json = Helper.toJson(true, "Salida registrada");

                    return new HttpResponseMessage()
                    {
                        Content = new StringContent(json.ToString(), Encoding.UTF8, "application/json")
                    };
                }
            }

            json = Helper.toJson(false, "Falta parametro o no existe la visita");

            return new HttpResponseMessage()
            {
                Content = new StringContent(json.ToString(), Encoding.UTF8, "application/json")
            };
        }

        [AllowAnonymous]
        [ActionName("CheckDom")]
        [HttpGet]
        public HttpResponseMessage CheckDom([FromUri] string Street = "", string Numberhouse = "")
        {
            var json = "";

            if (!String.IsNullOrEmpty(Street) && !String.IsNullOrEmpty(Numberhouse))
            {
                Guid Id = Guid.Parse(Street);
                var access = (from dom in db.Domicilios where dom.id_calle == Id && dom.no_casa == Numberhouse select dom).FirstOrDefault();

                if (access != null)
                {
                    json = Helper.toJson(true, "El domicilio si existe");

                    return new HttpResponseMessage()
                    {
                        Content = new StringContent(json.ToString(), Encoding.UTF8, "application/json")
                    };
                }
            }

            json = Helper.toJson(false, "No existe el domicilio");

            return new HttpResponseMessage()
            {
                Content = new StringContent(json.ToString(), Encoding.UTF8, "application/json")
            };
        }

        [AllowAnonymous]
        [HttpGet]
        [ActionName("Get")]
        public HttpResponseMessage GetCurrentAccess()
        {

            var datos = from subject in db.Asuntos from accesos in db.Accesos from domicilio in db.Domicilios from calle in db.Calles from visitante in db.Visitantes where accesos.fecha_salida==null && subject.id_asunto == accesos.id_asunto && visitante.id_visitante == accesos.id_visitante && accesos.id_domicilio == domicilio.id_domicilio && calle.id_calle == domicilio.id_calle select new AccessCurrentViewModel { Id = accesos.id_acceso, Plate = accesos.placa, Cone = accesos.cono, Name = visitante.nombre + " " + visitante.apellido1 + " " + visitante.apellido2, Street = calle.nombre, NumberHouse = domicilio.no_casa, Photo = visitante.foto  };

            return new HttpResponseMessage()
            {
                Content = new StringContent(JArray.FromObject(datos).ToString(), Encoding.UTF8, "application/json")
            };
        }
    }
}
