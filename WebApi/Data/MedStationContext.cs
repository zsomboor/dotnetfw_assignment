using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Models;

namespace WebApi.Data
{
    public class MedStationContext : DbContext
    {

        public MedStationContext() : base()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=MedStationDatabase;Trusted_Connection=True;");
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<CheckIn> CheckIns { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Patient>()
                .HasIndex(u => u.SocialSecurityId)
                .IsUnique();
        }
    }
}