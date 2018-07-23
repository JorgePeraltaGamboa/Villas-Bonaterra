using VillasBonaterra.Help;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using VillasBonaterra.ViewModels;
using System.Data.Entity.Validation;
using VillasBonaterra.Models;
using System.Web.Script.Serialization;
using System.Web;
using System.IO;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using Newtonsoft.Json.Linq;
using System.Web.Security;
using VillasBonaterra.Seguridad;

namespace VillasBonaterra.ApiControllers
{
    public class VisitorController : ApiController
    {
        Villas_BonaterraEntities db = new Villas_BonaterraEntities();

        [Authorize]
        [ActionName("Update")]
        [HttpPost]
        public async Task<HttpResponseMessage> UpdateAsync()
        {
            var folderName = "Content/Photos";
            var CurrentTime = Helper.Now();
            string json = "";

            var PATH = HttpContext.Current.Server.MapPath("~/" + folderName);
            var rootUrl = Request.RequestUri.AbsoluteUri.Replace(Request.RequestUri.AbsolutePath, String.Empty);
            var streamProvider = new MultipartFormDataStreamProvider(PATH);

            
            if (Request.Content.IsMimeMultipartContent())
            {
                await Request.Content.ReadAsMultipartAsync(streamProvider);

                var Id = Guid.Parse(streamProvider.FormData["Id"]);
                var Name = streamProvider.FormData["Name"];
                var MidleName = streamProvider.FormData["MidleName"];
                var LastName = streamProvider.FormData["LastName"];
                var BirthDay = streamProvider.FormData["BirthDay"];
                var Checked = streamProvider.FormData["Verificado"];
                var Photo = streamProvider.FormData["Photo"];

                Visitantes newVisitor = db.Visitantes.FirstOrDefault(s => s.id_visitante == Id);

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

                    File.Move(fileData.LocalFileName, Path.Combine(PATH, phototemp));

                    if (File.Exists(Path.Combine(PATH, newVisitor.foto)))
                    {
                        File.Delete(Path.Combine(PATH, newVisitor.foto));
                    }
                    if (File.Exists(Path.Combine(PATH, "min" + newVisitor.foto)))
                    {
                        File.Delete(Path.Combine(PATH, "min" + newVisitor.foto));
                    }

                    Photo = phototemp;

                    Image LastImage = Image.FromFile(Path.Combine(PATH, phototemp), true);
                    Helper.SaveJpeg(Path.Combine(PATH, "min" + phototemp), LastImage, 40);
                    LastImage.Dispose();
                }

                try
                {
                    newVisitor.nombre = Name;
                    newVisitor.apellido1 = MidleName;
                    newVisitor.apellido2 = LastName;
                    bool aux;
                    if (Boolean.TryParse(Checked,out aux)) {
                        newVisitor.verificado = Boolean.Parse(Checked);
                        VerificacionVisitantes very = new VerificacionVisitantes();
                        very.IdVerificacion = Guid.NewGuid();
                        
                        var CurrentUser =  (IdentityPersonalizado)User.Identity;
                        very.IdUsuario = CurrentUser.Id;
                        
                        very.IdVisitante = newVisitor.id_visitante;
                        very.Fecha = Helper.Now();

                        db.VerificacionVisitantes.Add(very);

                    }
                    newVisitor.nacimiento = DateTime.ParseExact(BirthDay, @"d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    newVisitor.foto = Photo == null ? newVisitor.foto: Photo;
                    
                    db.SaveChanges();

                    json = Helper.toJson(true, "Visitante Actualizado");

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
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }

                        json = Helper.toJson(false, "No se pudo actualizar visitante");

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
                
                var Name = streamProvider.FormData["Name"];
                var MidleName = streamProvider.FormData["MidleName"];
                var LastName = streamProvider.FormData["LastName"];
                var BirthDay = streamProvider.FormData["BirthDay"];
                var Photo = streamProvider.FormData["Photo"];

                Visitantes newVisitor = new Visitantes();

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

                    string base64temp = text.Split(',')[0];
                    string base64 = text.Split(',')[1];

                    if (base64temp == "data:image/jpeg;base64")
                    {
                        fromAndroid = true;
                        byte[] bytes = Convert.FromBase64String(base64);
                        using (Image image = Image.FromStream(new MemoryStream(bytes)))
                        {
                            image.Save(Path.Combine(PATH, photo), ImageFormat.Jpeg);  //
                        }
                    }
                    else {
                        File.Move(Path.Combine(PATH, phototemp), Path.Combine(PATH, photo));
                    }
                    // First load the image somehow
                    Image myImage = Image.FromFile(Path.Combine(PATH, photo), true);

                    if (myImage.Width < myImage.Height)
                    {
                        Helper.RotateAndSaveImage(Path.Combine(PATH, photo), Path.Combine(PATH, lastphoto));
                    }
                    else {
                        myImage.Save(Path.Combine(PATH, lastphoto), ImageFormat.Jpeg);
                        
                    }
                    // Save the image with a quality of 50%
                    myImage.Dispose();

                    /*
                    Image LastImage = Image.FromFile(Path.Combine(PATH, lastphoto), true);
                    Helper.SaveJpeg(Path.Combine(PATH, "min" + lastphoto), LastImage, 40);
                    LastImage.Dispose();
                    */

                    Image imageX = Image.FromFile(Path.Combine(PATH, lastphoto), true);
                    Image thumb = imageX.GetThumbnailImage(300, 180, () => false, IntPtr.Zero);
                    thumb.Save(Path.Combine(PATH, "min" + lastphoto));

                    thumb.Dispose();


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
                    var pins = (from vis in db.Visitantes select vis.pin).ToArray();

                    newVisitor.pin = Helper.GetPIN(pins);
                    newVisitor.id_visitante = Guid.Parse(Guid.NewGuid().ToString().ToUpper());
                    newVisitor.nombre = Name;
                    newVisitor.apellido1 = MidleName;
                    newVisitor.apellido2 = LastName;

                    newVisitor.nacimiento = DateTime.ParseExact(BirthDay, @"d/M/yyyy",System.Globalization.CultureInfo.InvariantCulture);

                    Photo = Photo == "undefined" ? "" : Photo;
                    newVisitor.foto = Photo == null ? "xxx" : Photo;

                    db.Visitantes.Add(newVisitor);

                    db.SaveChanges();

                    if (fromAndroid)
                    {
                        json = Helper.toJson(true, newVisitor.pin);
                    }
                    else {
                        json = Helper.toJson(true, "Visitante Agregado");
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

                        json = Helper.toJson(false, "No se pudo agregar visitante");

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
        [HttpGet]
        [ActionName("Get")]
        public HttpResponseMessage GetVisitor(string id = "")
        {
            var json = "";
            List<LastVisitorModel> visitors = new List<LastVisitorModel>();

            if (!String.IsNullOrEmpty(id)) {
                    var isNumeric = int.TryParse(id, out int n);

                if (isNumeric)
                {
                    var aux = (from us in db.Visitantes where us.pin == id select us).FirstOrDefault();

                    if (aux != null)
                    {
                        LastVisitorModel last = new LastVisitorModel();
                        last.PIN = aux.pin;
                        last.Name = aux.nombre;
                        last.MidleName = aux.apellido1;
                        last.LastName = aux.apellido2;
                        last.BirthDay = aux.nacimiento;
                        last.Photo = aux.foto;
                        last.Plate = "";

                        var lastaccess = (from access in db.Accesos where access.id_visitante == aux.id_visitante orderby access.fecha descending select access).FirstOrDefault();

                        if (lastaccess != null) {
                            last.LastAccess = lastaccess.fecha;
                        }
                        else {
                            last.LastAccess = null;
                        }

                        visitors.Add(last);
                    }
                }
                else {

                    if (Helper.isPlate(id))
                    {
                        var users = (from ac in db.Accesos from vi in db.Visitantes where ac.id_visitante == vi.id_visitante && ac.placa == id group ac by ac.id_visitante into g select new { Id = g.Key, Fecha = g.Max(q => q.fecha) }).OrderByDescending(f => f.Fecha).ToList();


                        for (var i = 0; i < 5 && i < users.Count(); i++)
                        {

                            var aux = db.Visitantes.Find(users[i].Id);
                            LastVisitorModel last = new LastVisitorModel();
                            last.PIN = aux.pin;
                            last.Name = aux.nombre;
                            last.MidleName = aux.apellido1;
                            last.LastName = aux.apellido2;
                            last.BirthDay = aux.nacimiento;
                            last.Photo = aux.foto;
                            last.Plate = id;
                            last.LastAccess = users[i].Fecha;

                            visitors.Add(last);
                        }
                    }
                    else
                    {
                        // Search by Name
                        var users = (from vi in db.Visitantes where vi.nombre.ToLower().Contains(id.ToLower()) || vi.apellido1.ToLower().Contains(id.ToLower()) || vi.apellido2.ToLower().Contains(id.ToLower()) orderby vi.nombre, vi.apellido1, vi.apellido2 descending select vi).ToList();
                        
                        for (var i = 0; i < 5 && i < users.Count(); i++)
                        {

                            var aux = db.Visitantes.Find(users[i].id_visitante);
                            LastVisitorModel last = new LastVisitorModel();
                            last.PIN = aux.pin;
                            last.Name = aux.nombre;
                            last.MidleName = aux.apellido1;
                            last.LastName = aux.apellido2;
                            last.BirthDay = aux.nacimiento;
                            last.Photo = aux.foto;
                            last.Plate = "";
                            last.LastAccess = null;

                            visitors.Add(last);
                        }
                    }
                }
                

                return new HttpResponseMessage()
                {
                    Content = new StringContent(JArray.FromObject(visitors).ToString(), Encoding.UTF8, "application/json")
                };

            }

            json = Helper.toJson(false, "Falta parametro o no existe el visitante");

            return new HttpResponseMessage()
            {
                Content = new StringContent(json.ToString(), Encoding.UTF8, "application/json")
            };
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}
