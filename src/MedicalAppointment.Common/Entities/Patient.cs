using System;
using System.Collections.Generic;

namespace MedicalAppointment.Common.Entities
{
    public class Patient : BaseEntity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime BirthDate { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string ZipCode { get; set; }

        public string HealthInsurance { get; set; }

        public string Phone { get; set; }

        public virtual List<Appointment> Appointments { get; set; }
    }
}
