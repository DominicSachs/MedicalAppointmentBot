using System;
using System.Linq;
using MedicalAppointment.Common.Entities;
using MedicalAppointment.Common.Models;
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
            

            

            var p1 = new Patient
            {
                Id = 1,
                FirstName = "Tina",
                LastName = "Tester",
                BirthDate = DateTime.Parse("02.02.1975"),
                City = "Leipzig",
                Phone = "1231",
                HealthInsurance = "AOK",
                CreatedBy = "Migration",
                CreationDate = DateTime.Now,
                ModifiedBy = "Migration",
                ModifiedDate = DateTime.Now
            };
            var p2 = new Patient
            {
                Id = 2,
                FirstName = "Sam",
                LastName = "Sample",
                BirthDate = DateTime.Parse("02.02.1982"),
                City = "Berlin",
                Phone = "4321",
                HealthInsurance = "KKH",
                CreatedBy = "Migration",
                CreationDate = DateTime.Now,
                ModifiedBy = "Migration",
                ModifiedDate = DateTime.Now
            };

            builder.Entity<Patient>().HasData(p1, p2);
            builder.Entity<Appointment>().HasData(
                new Appointment
                {
                    Id = 1,
                    PatientId = 1,
                    AppointmentStart = DateTime.Parse("02.12.2018 12:30:00"),
                    AppointmentEnd = DateTime.Parse("02.12.2018 13:00:00"),
                    State = AppointmentState.Active,
                    Reason = AppointmentReason.MedicalExamination,
                    CreatedBy = "Migration",
                    CreationDate = DateTime.Now,
                    ModifiedBy = "Migration",
                    ModifiedDate = DateTime.Now
                },
                new Appointment
                {
                    Id = 2,
                    PatientId = 1,
                    AppointmentStart = DateTime.Parse("12.04.2019 08:00:00"),
                    AppointmentEnd = DateTime.Parse("12.04.2018 08:30:00"),
                    State = AppointmentState.Active,
                    Reason = AppointmentReason.MedicalExamination,
                    CreatedBy = "Migration",
                    CreationDate = DateTime.Now,
                    ModifiedBy = "Migration",
                    ModifiedDate = DateTime.Now
                }
            );
        }
    }
}
