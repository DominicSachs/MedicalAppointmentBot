using System.Collections.Generic;
using System.Threading.Tasks;
using MedicalAppointment.App.Models;
using Microsoft.Bot.Builder;
using PromptsDialog = Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Prompts;
using Microsoft.Bot.Builder.Prompts.Choices;
using Microsoft.Recognizers.Text;

namespace MedicalAppointment.App.Bots.Dialogs
{
    internal class ChoicePromptDialog
    {
        public static string Id => "ChoicePrompt";

        internal static PromptsDialog.IDialog GetPromptDialog()
        {
            return new PromptsDialog.ChoicePrompt(Culture.German, AppointmentChoiceValidator);
        }

        internal static Task GetCardStep(PromptsDialog.DialogContext dialogContext, object result, PromptsDialog.SkipStepFunction next)
        {
            var cardOptions = new PromptsDialog.ChoicePromptOptions
            {
                Choices = new List<Choice>
                {
                    new Choice
                    {
                        Value = "Terminanfrage",
                        Synonyms = new List<string> { AppointmentType.Create.ToString() }
                    },
                    new Choice
                    {
                        Value = "Terminabsage",
                        Synonyms = new List<string> { AppointmentType.Cancel.ToString() }
                    }
                }
            };

            return dialogContext.Prompt(Id, "Was möchten Sie tun?", cardOptions);
        }

        private static async Task AppointmentChoiceValidator(ITurnContext context, ChoiceResult result)
        {
            if (!result.Succeeded())
            {
                result.Status = PromptStatus.NotRecognized;
                await context.SendActivity("Asuwahl nicht erkannt.");
            }
            var value = result.Value;
        }
    }
}
