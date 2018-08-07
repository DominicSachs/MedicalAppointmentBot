using System;
using System.Linq;
using MedicalAppointment.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace MedicalAppointment.Common.Data
{
    internal class AppDbContext : DbContext
    {
        public AppDbContext() { }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Patient> Patients { get; set; }

        public DbSet<Appointment> Appointments { get; set; }

        public override int SaveChanges()
        {
            var addedEntries = ChangeTracker.Entries<BaseEntity>().Where(x => x.State == EntityState.Added);

            foreach (var item in addedEntries)
            {
                item.Property(nameof(BaseEntity.CreationDate)).CurrentValue = DateTime.Now;
                item.Property(nameof(BaseEntity.ModifiedDate)).CurrentValue = DateTime.Now;
            }

            var modifiedEntries = ChangeTracker.Entries<BaseEntity>().Where(x => x.State == EntityState.Modified);

            foreach (var item in modifiedEntries)
            {
                item.Property(nameof(BaseEntity.ModifiedDate)).CurrentValue = DateTime.Now;
            }

            return base.SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<BaseEntity>().HasKey(p => p.Id);
            builder.Entity<BaseEntity>().Property(p => p.CreatedBy).IsRequired().HasMaxLength(256);
            builder.Entity<BaseEntity>().Property(p => p.ModifiedBy).IsRequired().HasMaxLength(256);
            builder.Entity<BaseEntity>().Property(p => p.CreationDate).IsRequired();
            builder.Entity<BaseEntity>().Property(p => p.ModifiedDate).IsRequired();

            builder.Entity<Patient>().Property(p => p.FirstName).IsRequired().HasMaxLength(256);
            builder.Entity<Patient>().Property(p => p.FirstName).IsRequired().HasMaxLength(256);
            builder.Entity<Patient>().Property(p => p.BirthDate).IsRequired();
            builder.Entity<Patient>().Property(p => p.HealthInsurance).IsRequired().HasMaxLength(128);
            builder.Entity<Patient>().Property(p => p.Phone).IsRequired().HasMaxLength(128);

            builder.Entity<Appointment>().Property(p => p.AppointmentStart).IsRequired();
            builder.Entity<Appointment>().Property(p => p.AppointmentEnd).IsRequired();
            builder.Entity<Appointment>().Property(p => p.State).IsRequired();

        }
    }
}
