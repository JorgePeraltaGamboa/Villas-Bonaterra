using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using VillasBonaterra.Models;
using VillasBonaterra.ViewModels;
using VillasBonaterra.Help;
using System.Data.Entity.Validation;
using System.Diagnostics;
using VillasBonaterra.Utilidades;
using System.Net.Mail;

namespace VillasBonaterra.Controllers
{
    public class LoginController : Controller
    {
        Villas_BonaterraEntities db = new Villas_BonaterraEntities();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Usuario model)
        {

            if (Membership.ValidateUser(model.login, model.password))
            {
                if (Helper.CheckEmailConfirmation(model.login))
                {
                    FormsAuthentication.RedirectFromLoginPage(model.login, true);
                    FormsAuthentication.SetAuthCookie(model.login, true);
                    return null;
                }
                ModelState.AddModelError("", "Es necesario confirmar tu correo");
            }
            else {
                ModelState.AddModelError("", "Usuario y/o contraseña incorrectos");
            } 
        return View(model);
        }

        public ActionResult Register() {
            return View();
        }

        [HttpGet]
        public ActionResult ConfirmEmail(ConfirmEmailModel model) {
            if (ModelState.IsValid) {
                var user = db.Usuario.FirstOrDefault(w => w.id.ToString().ToLower() == model.Token.ToLower() && w.email == model.Email);
                if (user != null) {
                    user.confirm_email = true;
                    db.SaveChanges();
                    return View();
                }
            }
            return RedirectToAction("Index","Login");
        }

        [HttpPost]
        public ActionResult Register(RegisterUserModel model)
        {
            var Json = "";
            if (ModelState.IsValid)
            {
                var user = (from u in db.Usuario where u.email.ToLower() == model.Email.ToLower() select u).FirstOrDefault();
                if (user != null)
                {
                    Json = Helper.toJson(false, "El correo ya esta registrado");
                }
                else {
                    var role = db.Roles.FirstOrDefault(x => x.nombre=="user");
                    
                    Usuario us = new Usuario { id= Helper.GuidUpper(),  login=model.Email, nombre=model.Name, apellido1= model.LastName, email= model.Email, password=SeguridadUtilidades.GetSha1(model.Password)};
                    UsuariosRoles rolex = new UsuariosRoles {id = Helper.GuidUpper(), IdUsuario=us.id, IdRole= role.id };
                 
                    us.UsuariosRoles.Add(rolex);
                    db.Usuario.Add(us);
                    try
                    {
                        db.SaveChanges();
                        Helper.SendEmailConfirmation(us, Url.Action("ConfirmEmail", "Login", new { Token = us.id, Email = us.email }, Request.Url.Scheme));
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
                        }
                        throw;
                    }

                    Json = Helper.toJson(true, "Registrado con exito!");
                }
                
            }
            else {
                var message = string.Join(" | ", ModelState.Values
                                           .SelectMany(v => v.Errors)
                                           .Select(e => e.ErrorMessage));
                Json = Helper.toJson(false, message);
            }
                return Content(Json.ToString(), "application/json");
            //return RedirectToAction("RegisterConfirmation","Login");
        }

        [HttpGet]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = (from u in db.Usuario where u.id.ToString().ToLower() == model.Token.ToString().ToLower() select u).FirstOrDefault();
                if (user == null)
                {
                    ModelState.AddModelError("", "El registro no existe!");
                }
                else
                {
                    var recovery = db.RecoveryPassword.FirstOrDefault(x => x.IdUser == user.id && x.Code==model.Code);

                    if (recovery == null)
                    {
                        ModelState.AddModelError("", "El registro no existe!");
                    }
                    else {
                        DateTime currentTime = Helper.Now();

                        if (recovery.Used == true || recovery.Date.AddMinutes(10) < currentTime )
                        {
                            ModelState.AddModelError("", "Link Invalido, vuelve a tratar de recuperar tu contraseña");
                        }
                        return View();
                    }
                }
            }
            return View(model);
            
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordConfirmModel model)
        {
            if (ModelState.IsValid)
            {
                var user = (from u in db.Usuario where u.id.ToString().ToLower() == model.Token.ToString().ToLower() select u).FirstOrDefault();
                if (user == null)
                {
                    ModelState.AddModelError("", "El registro no existe!");
                }
                else
                {
                    var recovery = db.RecoveryPassword.FirstOrDefault(x => x.IdUser == user.id && x.Code == model.Code);

                    if (recovery == null)
                    {
                        ModelState.AddModelError("", "El registro de recuperacion no existe!");
                    }
                    else
                    {
                        DateTime currentTime = Helper.Now();

                        if (recovery.Used == true || recovery.Date.AddMinutes(10) < currentTime)
                        {
                            ModelState.AddModelError("", "Tiempo para cambiar contraseña se termino, vuelve a tratar de recuperar tu contraseña");
                        }
                        else {
                            if (model.Password != model.ConfirmPassword) {
                                ModelState.AddModelError("", "Las contraseñas no son iguales");
                                return View(model);
                            }
                        }
                        recovery.Used = true;
                        user.password = Utilidades.SeguridadUtilidades.GetSha1(model.Password);
                        db.SaveChanges();
                        return RedirectToAction("ChangePasswordConfirm", "Login");
                    }
                }
            }
            return View(model);

        }

        public ActionResult ChangePasswordConfirm() {
            return View();
        }
        public ActionResult RegisterConfirmation()
        {
            return View();
        }

        public ActionResult Recovery() {
            return View();
        }
        
        [HttpPost]
        public ActionResult Recovery(LoginRecoveryModel model) {
            string Json = "";
            if (ModelState.IsValid)
            {
                var user = db.Usuario.FirstOrDefault(u => u.email.ToLower() == model.email.ToLower());

                if (user == null)
                {
                    Json = Helper.toJson(false, "No existe la cuenta de correo");
                }
                else {
                    RecoveryPassword rp = new RecoveryPassword(user.id);
                    //user.RecoveryPassword.Add(rp);
                    Helper.SendEmailRecovery(user, Url.Action("ChangePassword", "Login", new { Token = user.id, Code = rp.Code }, Request.Url.Scheme));
                    db.RecoveryPassword.Add(rp);

                    try {
                        db.SaveChanges();
                        Json = Helper.toJson(true, "");
                    }
                    catch (DbEntityValidationException e)
                    {
                        foreach (var eve in e.EntityValidationErrors)
                        {
                            Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                            foreach (var ve in eve.ValidationErrors)
                            {
                                Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                            }
                        }
                    throw;
                    }
            }
            }
            else {
                var message = string.Join(" | ", ModelState.Values
                                            .SelectMany(v => v.Errors)
                                            .Select(e => e.ErrorMessage));
                Json = Helper.toJson(false,message);
            }
            return Content(Json.ToString(), "application/json");

        }

        public ActionResult RecoverySended() {
            return View();
        }

        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
        

    }
}