﻿using System;
using MedicalAppointment.Common.Models;

namespace MedicalAppointment.Common.Entities
{
    public class Appointment : BaseEntity
    {
        public int PatientId { get; set; }
        
        public Patient Patient { get; set; }
        
        public AppointmentState State{ get; set; }

        public DateTime AppointmentStart { get; set; }

        public DateTime AppointmentEnd { get; set; }
    }
}
