using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;

namespace VillasBonaterra.Seguridad
{
    public class IdentityPersonalizado: IIdentity
    {
        public string Name {
            get { return login; }
        }

        public Guid Id
        {
            get { return id; }
        }
        public string AuthenticationType {
            get { return Identity.AuthenticationType;  }
        }
        public bool IsAuthenticated {
            get { return Identity.IsAuthenticated; }
        }

        public Guid id { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public string nombre { get; set; }
        public string apellidos { get; set; }

        public IIdentity Identity { get; set; }

        

        public IdentityPersonalizado(IIdentity identity) {
            this.Identity = identity;
            var us = Membership.GetUser(identity.Name) as UsuarioMembership;
            this.id = us.id;
            this.login = us.login;
        }
    }
}