using MedicalAppointment.Common.Data;
using MedicalAppointment.Common.Entities;
using MedicalAppointment.Common.Storage.Interfaces;

namespace MedicalAppointment.Common.Storage.Implementations
{
    public class AppointmentStorage : Storage<Appointment>, IAppointmentStorage
    {
        public AppointmentStorage(AppDbContext context) : base(context) { }
    }
}