using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using VillasBonaterra.Models;

namespace VillasBonaterra.Seguridad
{
    public class UsuarioMembership:MembershipUser
    {
        public Guid id { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public string nombre { get; set; }
        public string apellido1 { get; set; }
        public string apellido2 { get; set; }

        public UsuarioMembership(Usuario usuario) {
            id = usuario.id;
            login = usuario.login;
            password = usuario.password;
            nombre = usuario.nombre;
            apellido1 = usuario.apellido1;
            apellido2 = usuario.apellido2;
        }
    }
}