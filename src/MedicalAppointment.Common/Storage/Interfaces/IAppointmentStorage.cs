using System;
using System.Threading.Tasks;
using MedicalAppointment.Common.Entities;

namespace MedicalAppointment.Common.Storage.Interfaces
{
    public interface IAppointmentStorage : IStorage<Appointment>
    {
    }
}