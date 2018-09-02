using MedicalAppointment.App.Models;
using MedicalAppointment.Common.Extensions;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Prompts.Choices;
using Microsoft.Recognizers.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using Prompts = Microsoft.Bot.Builder.Prompts;

namespace MedicalAppointment.App.Bots.Dialogs
{
    internal class AppointmentPromptDialog : IPromptDialog
    {
        public string Name => nameof(AppointmentPromptDialog);

        public IDialog GetDialog() => new ChoicePrompt(Culture.German, ChoiceValidator) { Style = Prompts.ListStyle.SuggestedAction };

        public Task GetDialogStep(DialogContext dialogContext, object result, SkipStepFunction next)
        {
            return dialogContext.Prompt(Name, "Was möchten Sie tun?", GetOptions());
        }

        private static async Task ChoiceValidator(ITurnContext context, Prompts.ChoiceResult result)
        {
            if (!result.Succeeded())
            {
                result.Status = Prompts.PromptStatus.NotRecognized;
                await context.SendActivity("Asuwahl nicht erkannt.");
            }

            var state = context.GetConversationState<InMemoryPromptState>();
            state.AppointmentType = result.Value.Value.GetValueFromDescription<AppointmentType>();
        }

        private static ChoicePromptOptions GetOptions()
        {
            return new ChoicePromptOptions
            {
                Choices = new List<Choice>
                {
                    new Choice
                    {
                        Value = AppointmentType.Create.GetDescription(),
                        Synonyms = new List<string> { AppointmentType.Create.ToString(), nameof(AppointmentType.Create) }
                    },
                    new Choice
                    {
                        Value = AppointmentType.Cancel.GetDescription(),
                        Synonyms = new List<string> { AppointmentType.Cancel.ToString(), nameof(AppointmentType.Cancel) }
                    }
                }
            };
        }
    }
}
