using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using BPMLab.Turboreactors.Models;

namespace BPMLab.Turboreactors.DAL
{
    public class TurboreactorsContext : DbContext
    {
        public TurboreactorsContext() : base("TurboreactorsContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Turboreactor>()
                .HasMany(t => t.Types).WithMany(t => t.Turboreactors)
                .Map(t => t.MapLeftKey("TurboreactorID")
                    .MapRightKey("TurboreactorTypeID")
                    .ToTable("TurboreactorTurboreactorType"));

            modelBuilder.Entity<StoredFile>().MapToStoredProcedures();
            modelBuilder.Entity<Manufacture>().MapToStoredProcedures();
            modelBuilder.Entity<Turboreactor>().MapToStoredProcedures();
            modelBuilder.Entity<TurboreactorType>().MapToStoredProcedures();
        }

        public DbSet<StoredFile> Images { get; set; }
        public DbSet<Manufacture> Manufactures { get; set; }
        public DbSet<Turboreactor> Turboreactors { get; set; }
        public DbSet<TurboreactorType> TurboreactorTypes { get; set; }
    }

}