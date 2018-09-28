using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;

namespace MedicalAppointment.App.Bots.Dialogs
{
    public interface IPromptDialog
    {
        Dialog GetDialog();

        Task<DialogTurnResult> GetWaterfallStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken);
    }
}