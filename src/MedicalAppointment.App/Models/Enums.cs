using System.ComponentModel;

namespace MedicalAppointment.App.Models
{
    public enum AppointmentType
    {
        [Description("Terminanfrage")]
        Create,
        [Description("Terminabsage")]
        Cancel
    }
}
