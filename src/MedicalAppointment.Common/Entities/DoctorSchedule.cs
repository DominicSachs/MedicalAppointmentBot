using System;

namespace MedicalAppointment.Common.Entities
{
    public class DoctorSchedule : BaseEntity
    {
        public string DoctorId { get; set; }

        public Doctor Doctor { get; set; }

        public int DayOfWeek { get; set; }

        public TimeSpan StartTime { get; set; }
        
        public TimeSpan EndTime { get; set; }
        
        public TimeSpan BreakStartTime { get; set; }

        public TimeSpan BreakEndTime { get; set; }
    }
}
