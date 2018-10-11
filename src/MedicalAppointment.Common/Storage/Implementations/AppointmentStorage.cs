using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MedicalAppointment.Common.Data;
using MedicalAppointment.Common.Entities;
using MedicalAppointment.Common.Storage.Interfaces;

namespace MedicalAppointment.Common.Storage.Implementations
{
    public class AppointmentStorage : Storage<Appointment>, IAppointmentStorage
    {
        public AppointmentStorage(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Appointment>> GetFreeAppointments()
        {
            var now = DateTime.Now;
            return await Task.FromResult(
                new []
                {
                    new Appointment { AppointmentStart = now.AddDays(4)},
                    new Appointment { AppointmentStart = now.AddDays(5)},
                    new Appointment { AppointmentStart = now.AddDays(6)},
                    new Appointment { AppointmentStart = now.AddDays(7)},
                    new Appointment { AppointmentStart = now.AddDays(8)},
                    new Appointment { AppointmentStart = now.AddDays(9)},
                    new Appointment { AppointmentStart = now.AddDays(10)},
                    new Appointment { AppointmentStart = now.AddDays(14)},
                }
            );
        }
    }
}