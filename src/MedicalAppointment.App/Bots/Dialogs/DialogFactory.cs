using System.Collections.Generic;
using MedicalAppointment.Common.Storage.Interfaces;

namespace MedicalAppointment.App.Bots.Dialogs
{
    public class DialogFactory : IDialogFactory
    {
        public IEnumerable<IPromptDialog> GetDialogs(IPatientStorage patientStorage)
        {
            yield return new AppointmentPromptDialog();
            yield return new AppointmentReasonPromptDialog();
            yield return new NamePromptDialog();
            yield return new BithdatePromptDialog();
            yield return new AppointmentDatesToCancelDialog(patientStorage);
        }
    }
}