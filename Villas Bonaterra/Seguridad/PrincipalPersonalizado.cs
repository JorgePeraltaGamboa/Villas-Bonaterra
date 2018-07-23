using System;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using VillasBonaterra.Models;

namespace VillasBonaterra.Seguridad
{
    public class PrincipalPersonalizado:IPrincipal
    {
        public bool IsInRole(string role) {

            using (var db = new Villas_BonaterraEntities())
            {

                var usuario = Membership.GetUser(Identity.Name) as UsuarioMembership;
                var roles = db.Roles.FirstOrDefault(r => r.nombre.ToLower()==role.ToLower()); 

                if (roles != null && db.UsuariosRoles.Where(x => x.IdRole == roles.id && x.IdUsuario == usuario.id).Any())
                {
                    return true;
                }
                return false;
            }
        }

        public IIdentity Identity { get; private set; }

        public IdentityPersonalizado MiIdentityPersonalizada {
            get { return (IdentityPersonalizado)Identity; }
        }

        public PrincipalPersonalizado(IdentityPersonalizado identity) {
            Identity = identity;
        }
    }
}