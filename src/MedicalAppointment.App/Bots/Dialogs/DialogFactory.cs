using MedicalAppointment.Common.Storage.Interfaces;
using System.Collections.Generic;

namespace MedicalAppointment.App.Bots.Dialogs
{
    public class DialogFactory : IDialogFactory
    {
        public IEnumerable<IPromptDialog> GetDialogs(IPatientStorage patientStorage, IAppointmentStorage appointmentStorage, BotStateAccessors accessors)
        {
            yield return new AppointmentPromptDialog(accessors);
            yield return new AppointmentReasonPromptDialog(accessors);
            yield return new NamePromptDialog(accessors);
            yield return new BithdatePromptDialog(accessors);
            yield return new AppointmentDatesToCancelDialog(patientStorage, accessors);
            yield return new AppointmentDatesToConfirmDialog(patientStorage, appointmentStorage, accessors);
            yield return new AppointmentConfirmDialog(accessors);
        }
    }
}