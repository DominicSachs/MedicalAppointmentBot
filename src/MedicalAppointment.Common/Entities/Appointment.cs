using MedicalAppointment.Common.Models;
using System;

namespace MedicalAppointment.Common.Entities
{
    public class Appointment : BaseEntity
    {
        public int PatientId { get; set; }

        public virtual Patient Patient { get; set; }

        public AppointmentState State { get; set; }

        public AppointmentReason Reason { get; set; }

        public string Information { get; set; }

        public DateTime AppointmentStart { get; set; }

        public DateTime AppointmentEnd { get; set; }
    }
}
