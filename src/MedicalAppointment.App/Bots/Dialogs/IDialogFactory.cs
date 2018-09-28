using System.Collections.Generic;
using MedicalAppointment.Common.Storage.Interfaces;

namespace MedicalAppointment.App.Bots.Dialogs
{
    public interface IDialogFactory
    {
        IEnumerable<IPromptDialog> GetDialogs(IPatientStorage patientStorage, BotStateAccessors accessors);
    }
}