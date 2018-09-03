using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicalAppointment.Common.Entities;
using MedicalAppointment.Common.Models;
using Microsoft.Extensions.DependencyInjection;

namespace MedicalAppointment.Common.Data
{
    internal class DbInitializer
    {
        internal static async Task Seed(IServiceProvider provider)
        {
            var context = provider.GetRequiredService<AppDbContext>();

            if (!context.Patients.Any())
            {
                var p1 = new Patient
                {
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

                var a1 = new Appointment
                {
                    AppointmentStart = DateTime.Parse("02.12.2018 12:30:00"),
                    AppointmentEnd = DateTime.Parse("02.12.2018 13:00:00"),
                    State = AppointmentState.Active,
                    Reason = AppointmentReason.MedicalExamination,
                    CreatedBy = "Migration",
                    CreationDate = DateTime.Now,
                    ModifiedBy = "Migration",
                    ModifiedDate = DateTime.Now
                };

                var a2 = new Appointment
                {
                    AppointmentStart = DateTime.Parse("12.04.2019 08:00:00"),
                    AppointmentEnd = DateTime.Parse("12.04.2018 08:30:00"),
                    State = AppointmentState.Active,
                    Reason = AppointmentReason.MedicalExamination,
                    CreatedBy = "Migration",
                    CreationDate = DateTime.Now,
                    ModifiedBy = "Migration",
                    ModifiedDate = DateTime.Now
                };

                p1.Appointments = new List<Appointment> {a1, a2};

                context.Patients.Add(p1);
                context.Patients.Add(p2);

                await context.SaveChangesAsync();
            }
        }
    }
}
