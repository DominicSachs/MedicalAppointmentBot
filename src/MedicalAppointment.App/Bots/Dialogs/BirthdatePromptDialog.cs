using System;
using System.Linq;
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
        public string Name => nameof(BithdatePromptDialog);

        public PromptsDialog.IDialog GetDialog() => new PromptsDialog.DateTimePrompt(Culture.German, DateValidator);

        public async Task GetDialogStep(PromptsDialog.DialogContext dialogContext, object result, PromptsDialog.SkipStepFunction next)
        {
            //var state = dialogContext.Context.GetConversationState<InMemoryPromptState>();
            //state.Name = (result as TextResult)?.Value;
            await dialogContext.Prompt(Name, "Wie ist Ihr Geburtsdatum?");
        }

        private static async Task DateValidator(ITurnContext context, DateTimeResult result)
        {
            if (result.Resolution.Count == 0)
            {
                await context.SendActivity("Die Eingabe wurde nicht als Datum erkannt.");
                result.Status = PromptStatus.NotRecognized;
            }

            if (!result.Succeeded())
            {
                result.Status = PromptStatus.NotRecognized;
                await context.SendActivity("Sie haben kein gültiges Datum angegeben.");
            }

            // Find any recognized time that is not in the past.
            var now = DateTime.Now;
            var time = default(DateTime);
            var resolution = result.Resolution.FirstOrDefault(res => DateTime.TryParse(res.Value, out time));// && time > now);

            if (resolution != null)
            {
                var state = context.GetConversationState<InMemoryPromptState>();
                state.Birthdate = time;

                // If found, keep only that result.
                result.Resolution.Clear();
                result.Resolution.Add(resolution);
            }
            else
            {
                await context.SendActivity("Bitte geben Sie ein gültiges Datum an.");
                result.Status = PromptStatus.OutOfRange;
            }
        }
    }
}
