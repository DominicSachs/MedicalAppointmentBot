using System;
using MedicalAppointment.Common.Models;

namespace MedicalAppointment.App.Models
{
    public class UserProfile
    {
        public int PatientId { get; set; } 

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime Birthdate { get; set; }

        public AppointmentType AppointmentType { get; set; }

        public AppointmentReason AppointmentReason { get; set; }
    }
}
