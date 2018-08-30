using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;

namespace MedicalAppointment.App.Bots.Dialogs
{
    public interface IPromptDialog
    {
        string Name { get; }

        IDialog GetDialog();

        Task GetDialogStep(DialogContext dialogContext, object result, SkipStepFunction next);
    }
}