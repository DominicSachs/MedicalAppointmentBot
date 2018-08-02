using System;

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

        private string HealthInsurance { get; set; }
    }
}
