//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VillasBonaterra.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class TarjetasAcceso
    {
        public System.Guid IdCard { get; set; }
        public System.Guid IdDomicilio { get; set; }
        public string Numero { get; set; }
        public bool Activa { get; set; }
    
        public virtual Domicilios Domicilios { get; set; }
    }
}
