using System.Collections.Generic;

namespace MedicalAppointment.App.Bots.Dialogs
{
    public interface IDialogFactory
    {
        IEnumerable<IPromptDialog> GetDialogs();
    }
}