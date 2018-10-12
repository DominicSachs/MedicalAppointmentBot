using System.Collections.Generic;

namespace MedicalAppointment.Common.Entities
{
    public class Doctor : BaseEntity
    {
        public string Title { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        public virtual List<DoctorSchedule> Schedules { get; set; } = new List<DoctorSchedule>();
    }
}
