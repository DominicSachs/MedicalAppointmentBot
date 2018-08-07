using System.Threading.Tasks;
using MedicalAppointment.App.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Core.Extensions;
using PromptsDialog = Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Prompts;
using Microsoft.Recognizers.Text;

namespace MedicalAppointment.App.Bots.Dialogs
{
    internal class BithdatePromptDialog
    {
        public static string Id => "BirthdatePrompt";

        internal static PromptsDialog.IDialog GetPromptDialog()
        {
            return new PromptsDialog.DateTimePrompt(Culture.German, DateValidator);
        }
        
        internal static async Task GetCardStep(PromptsDialog.DialogContext dialogContext, object result, PromptsDialog.SkipStepFunction next)
        {
            var state = dialogContext.Context.GetConversationState<InMemoryPromptState>();
            state.Name = (result as TextResult)?.Value;
            await dialogContext.Prompt(Id, "Wie ist Ihr Geburtsdatum?");
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
