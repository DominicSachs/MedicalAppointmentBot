using System;
using System.Collections.Generic;
using MedicalAppointment.Common.Models;

namespace MedicalAppointment.App.Models
{
    public class InMemoryPromptState : Dictionary<string, object>
    {
        public string FirstName
        {
            get => (string)this[nameof(FirstName)];
            set => this[nameof(FirstName)] = value;
        }

        public string LastName
        {
            get => (string)this[nameof(LastName)];
            set => this[nameof(LastName)] = value;
        }

        public DateTime Birthdate
        {
            get => (DateTime)this[nameof(Birthdate)];
            set => this[nameof(Birthdate)] = value;
        }

        public AppointmentType AppointmentType
        {
            get => (AppointmentType)this[nameof(AppointmentType)];
            set => this[nameof(AppointmentType)] = value;
        }

        public AppointmentReason AppointmentReason
        {
            get => (AppointmentReason)this[nameof(AppointmentReason)];
            set => this[nameof(AppointmentReason)] = value;
        }
    }
}
