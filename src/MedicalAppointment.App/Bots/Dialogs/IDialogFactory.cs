using System.Collections.Generic;
using Microsoft.Bot.Builder.Dialogs;

namespace MedicalAppointment.App.Bots.Dialogs
{
    public interface IDialogFactory
    {
        IEnumerable<IPromptDialog> GetDialogs();
    }
}