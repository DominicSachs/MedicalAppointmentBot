using MedicalAppointment.App.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Prompts;
using Microsoft.Bot.Builder.Prompts.Choices;
using Microsoft.Recognizers.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using PromptsDialog = Microsoft.Bot.Builder.Dialogs;

namespace MedicalAppointment.App.Bots.Dialogs
{
    internal class ChoicePromptDialog : IPromptDialog
    {
        public string Name => "ChoicePrompt";

        public PromptsDialog.IDialog GetDialog() => new PromptsDialog.ChoicePrompt(Culture.German, ChoiceValidator);

        public Task GetDialogStep(PromptsDialog.DialogContext dialogContext, object result, PromptsDialog.SkipStepFunction next)
        {
            var cardOptions = new PromptsDialog.ChoicePromptOptions
            {
                Choices = new List<Choice>
                {
                    new Choice
                    {
                        Value = "Request an appointment ",
                        Synonyms = new List<string> { AppointmentType.Create.ToString() }
                    },
                    new Choice
                    {
                        Value = "Cancel an appointment",
                        Synonyms = new List<string> { AppointmentType.Cancel.ToString() }
                    }
                }
            };

            return dialogContext.Prompt("ChoicePrompt", "Please select an option.", cardOptions);
        }

        private static async Task ChoiceValidator(ITurnContext context, ChoiceResult result)
        {
            if (!result.Succeeded())
            {
                result.Status = PromptStatus.NotRecognized;
                await context.SendActivity("Unknown option.");
            }
            var value = result.Value;
        }
    }
}
