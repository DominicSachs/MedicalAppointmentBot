using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MedicalAppointment.Common.Entities;

namespace MedicalAppointment.Common.Storage.Interfaces
{
    public interface IAppointmentStorage : IStorage<Appointment>
    {
        Task<IEnumerable<Appointment>> GetFreeAppointments();
    }
}