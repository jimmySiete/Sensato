//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Sensato.GenerateCSharp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tb_Objects
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tb_Objects()
        {
            this.Tb_Parameters = new HashSet<Tb_Parameters>();
            this.Tb_ResultSets = new HashSet<Tb_ResultSets>();
        }
    
        public int ID_Object { get; set; }
        public int ID_Context { get; set; }
        public string ObjectName { get; set; }
        public long ID_SysObject { get; set; }
        public string ObjDescription { get; set; }
        public string Entity { get; set; }
        public string Sys_ObjectName { get; set; }
    
        public virtual Tb_Contexts Tb_Contexts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tb_Parameters> Tb_Parameters { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tb_ResultSets> Tb_ResultSets { get; set; }
    }
}
