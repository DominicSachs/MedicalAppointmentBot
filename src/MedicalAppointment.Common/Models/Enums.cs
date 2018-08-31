using System.ComponentModel;

namespace MedicalAppointment.Common.Models
{
    public enum AppointmentState
    {
        Active,
        PatientCanceled,
        DoctorCanceled,
        PatientNotAppeared,
        Done
    }

    public enum AppointmentReason
    {
        [Description("Akutsprechstunde")]
        UrgentSurgery,
        [Description("Beratung & Untersuchung")]
        MedicalExamination,
        [Description("Erstgespräch als Neupatient")]
        NewPatient
    }
}
