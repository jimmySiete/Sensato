﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DB_GeneratorEntities : DbContext
    {
        public DB_GeneratorEntities()
            : base("name=DB_GeneratorEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Tb_Contexts> Tb_Contexts { get; set; }
        public virtual DbSet<Tb_Objects> Tb_Objects { get; set; }
        public virtual DbSet<Tb_Parameters> Tb_Parameters { get; set; }
        public virtual DbSet<Tb_ResultSetColumns> Tb_ResultSetColumns { get; set; }
        public virtual DbSet<Tb_ResultSets> Tb_ResultSets { get; set; }
        public virtual DbSet<Tb_Projects> Tb_Projects { get; set; }
    }
}
