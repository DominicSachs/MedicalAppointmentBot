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
    }
}
