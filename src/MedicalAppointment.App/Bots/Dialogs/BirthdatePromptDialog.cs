using System.Threading.Tasks;
using MedicalAppointment.App.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Core.Extensions;
using PromptsDialog = Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Prompts;
using Microsoft.Recognizers.Text;

namespace MedicalAppointment.App.Bots.Dialogs
{
    internal class BithdatePromptDialog : IPromptDialog
    {
        public string Name => "BirthdatePrompt";

        public PromptsDialog.IDialog GetDialog() => new PromptsDialog.DateTimePrompt(Culture.German, DateValidator);

        public async Task GetDialogStep(PromptsDialog.DialogContext dialogContext, object result, PromptsDialog.SkipStepFunction next)
        {
            var state = dialogContext.Context.GetConversationState<InMemoryPromptState>();
            state.Name = (result as TextResult)?.Value;
            await dialogContext.Prompt(Name, "Wie ist Ihr Geburtsdatum?");
        }

        private static async Task DateValidator(ITurnContext context, DateTimeResult result)
        {
            if (result.Resolution.Count == 0)
            {
                await context.SendActivity("Sorry, I did not recognize the time that you entered.");
                result.Status = PromptStatus.NotRecognized;
            }

            if (!result.Succeeded())
            {
                result.Status = PromptStatus.NotRecognized;
                await context.SendActivity("Sie haben kein gültiges Datum angegeben.");
            }
        }
    }
}
