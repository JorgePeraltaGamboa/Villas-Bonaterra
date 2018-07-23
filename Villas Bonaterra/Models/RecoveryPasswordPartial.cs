using VillasBonaterra.Help;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace VillasBonaterra.Models
{
    public partial class RecoveryPassword
    {
        public RecoveryPassword(Guid IdUser)
        {
            using (var db = new Villas_BonaterraEntities())
            {
                var records = db.RecoveryPassword.Where(x => x.IdUser == IdUser).ToList();
                records.ForEach(z => z.Used = true);
                db.SaveChanges();
            }
            this.Id = Guid.NewGuid();
            this.Code = Guid.NewGuid();
            this.Date = Helper.Now();
            this.IdUser = IdUser;
        }
        public RecoveryPassword(){}
        
    }
}