﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Final_Project.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class testdbEntiies : DbContext
    {
        public testdbEntiies()
            : base("name=testdbEntiies")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<creator> creators { get; set; }
        public virtual DbSet<accfuctional> accfuctionals { get; set; }
        public virtual DbSet<@class> classes { get; set; }
        public virtual DbSet<hrfuctional> hrfuctionals { get; set; }
        public virtual DbSet<m> ms { get; set; }
        public virtual DbSet<roledata> roledatas { get; set; }
        public virtual DbSet<subject> subjects { get; set; }
        public virtual DbSet<role_funcdata> role_funcdata { get; set; }
        public virtual DbSet<studfuctional> studfuctionals { get; set; }
    }
}
