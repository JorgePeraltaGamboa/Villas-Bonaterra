using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;
using VillasBonaterra.Models;
using VillasBonaterra.Seguridad;

namespace VillasBonaterra.Help
{
    public class Helper
    {
        public static void RotateAndSaveImage(String input, String output)
        {
            //create an object that we can use to examine an image file
            using (Image img = Image.FromFile(input,true))
            {
                //rotate the picture by 90 degrees and re-save the picture as a Jpeg
                img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                img.Save(output, ImageFormat.Jpeg);
                
            }
        }
        /// <summary> 
        /// Saves an image as a jpeg image, with the given quality 
        /// </summary> 
        /// <param name="path"> Path to which the image would be saved. </param> 
        /// <param name="quality"> An integer from 0 to 100, with 100 being the highest quality. </param> 
        public static void SaveJpeg(string path, Image img, int quality)
        {
            if (quality < 0 || quality > 100)
                throw new ArgumentOutOfRangeException("quality must be between 0 and 100.");

            // Encoder parameter for image quality 
            EncoderParameter qualityParam = new EncoderParameter(Encoder.Quality, quality);
            // JPEG image codec 
            ImageCodecInfo jpegCodec = GetEncoderInfo("image/jpeg");
            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;
            img.Save(path, jpegCodec, encoderParams);
        }

        /// <summary> 
        /// Returns the image codec with the given mime type 
        /// </summary> 
        private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            // Get image codecs for all image formats 
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            // Find the correct image codec 
            for (int i = 0; i < codecs.Length; i++)
                if (codecs[i].MimeType == mimeType)
                    return codecs[i];

            return null;
        }
        public static string toJson(bool flag, string message)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(new { success = flag, message = message });
        }

        public static Guid GuidUpper() {
            return Guid.Parse(Guid.NewGuid().ToString().ToUpper());
        }

        internal static void SendEmailRecovery(Usuario us, string Url)
        {
            try
            {
                string fromEmail = ConfigurationManager.AppSettings["from"];
                string displayEmailRecovery = ConfigurationManager.AppSettings["displayEmailRecovery"];
                string subjectRecovery = ConfigurationManager.AppSettings["subjectRecovery"];
                string textLinkConfigRecovery = ConfigurationManager.AppSettings["textLinkRecovery"];
                string textBodyRecovery = ConfigurationManager.AppSettings["textBodyRecovery"];

                string textLink = string.Format("<a href =\"{0}\" title =\"Recovery Password\">{1}</a>", Url, textLinkConfigRecovery);

                MailMessage m = new MailMessage(new MailAddress(fromEmail, displayEmailRecovery), new MailAddress(us.email));
                m.Subject = subjectRecovery;
                m.Body = string.Format(textBodyRecovery, us.nombre + " " + us.apellido1 + " " + us.apellido2 , textLink);
                m.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["ServerEmail"], Int32.Parse(ConfigurationManager.AppSettings["ServerPort"]));
                smtp.UseDefaultCredentials = false;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["emailUser"], ConfigurationManager.AppSettings["emailPass"]);
                smtp.EnableSsl = Boolean.Parse(ConfigurationManager.AppSettings["ServerSSL"]);
                smtp.Send(m);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR: " + ex.Message);
            }
        }

        internal static void SendEmailConfirmation(Usuario us,string Url)
        {
            try
            {
                string fromEmail = ConfigurationManager.AppSettings["from"];
                string displayEmail = ConfigurationManager.AppSettings["displayEmail"];
                string subject = ConfigurationManager.AppSettings["subject"];
                string textLinkConfig = ConfigurationManager.AppSettings["textLink"];
                string textBody = ConfigurationManager.AppSettings["textBody"];

                string textLink = string.Format("<a href =\"{0}\" title =\"User Email Confirm\">{1}</a>",Url,textLinkConfig);

                MailMessage m = new MailMessage(new MailAddress(fromEmail, displayEmail), new MailAddress(us.email));
                m.Subject = subject;
                m.Body = string.Format(textBody, us.nombre + " " + us.apellido1 + " " + us.apellido2, textLink);
                m.IsBodyHtml = true;
                
                SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["ServerEmail"], Int32.Parse(ConfigurationManager.AppSettings["ServerPort"]));
                smtp.UseDefaultCredentials = false;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["emailUser"], ConfigurationManager.AppSettings["emailPass"]);
                smtp.EnableSsl = Boolean.Parse(ConfigurationManager.AppSettings["ServerSSL"]);
                smtp.Send(m);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR: " + ex.Message);
            }
        }

        internal static bool CheckEmailConfirmation(string login)
        {
            using (var db = new Villas_BonaterraEntities())
            {
                var user = db.Usuario.FirstOrDefault(o => o.login.ToLower() == login.ToLower() && o.confirm_email == true);

                if (user == null)
                {
                    user = db.Usuario.FirstOrDefault(o => o.email.ToLower() == login.ToLower() && o.confirm_email == true);
                }

                if (user != null)
                {
                    return true;
                }
            }
            return false;
        }

        internal static DateTime Now()
        {

            TimeZoneInfo setTimeZoneInfo;
            DateTime currentDateTime;

            //Set the time zone information to US Mountain Standard Time 
            setTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time (Mexico)");

            //Get date and time in US Mountain Standard Time
            currentDateTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, setTimeZoneInfo);

            //Este cambio es paar que se muestren todas las citas proximas agendadas
            //DateTime start = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day); //Today at 00:00:00
            return currentDateTime;
        }


        internal static string GetPIN(string[] pins)
        {
            Random rnd = new Random();
            var pin = rnd.Next(1000, 10000).ToString();
            var flag = true;

            if (pins != null)
            {
                while (flag) {
                    if (pins.Contains(pin))
                    {
                        pin = rnd.Next(1000, 10000).ToString();
                    }
                    else {
                        flag = false;
                    }
                }
                
            }
            return pin;
        }

        internal static bool IsBase64(string base64String)
        {
            if (base64String == null || base64String.Length == 0 || base64String.Length % 4 != 0 || base64String.Contains(" ") || base64String.Contains("\t") || base64String.Contains("\r") || base64String.Contains("\n"))
                return false;
            return true;
        }

        internal static bool isPlate(string id)
        {
            return id.Any(char.IsDigit);
        }

        public static bool IsInRole(string name, string role)
        {

            using (var db = new Villas_BonaterraEntities())
            {

                var usuario = Membership.GetUser(name) as UsuarioMembership;
                var roles = db.Roles.FirstOrDefault(r => r.nombre.ToLower() == role.ToLower());

                if (roles != null && db.UsuariosRoles.Where(x => x.IdRole == roles.id && x.IdUsuario == usuario.id).Any())
                {
                    return true;
                }
                return false;
            }
        }
    }
}