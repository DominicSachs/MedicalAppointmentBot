using System.Collections.Generic;
using System.Threading.Tasks;
using MedicalAppointment.App.Models;
using MedicalAppointment.Common.Extensions;
using MedicalAppointment.Common.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Prompts.Choices;
using Microsoft.Recognizers.Text;
using Prompts = Microsoft.Bot.Builder.Prompts;

namespace MedicalAppointment.App.Bots.Dialogs
{
    internal class AppointmentReasonPromptDialog : IPromptDialog
    {
        public string Name => nameof(AppointmentReasonPromptDialog);

        public IDialog GetDialog() => new ChoicePrompt(Culture.German, ChoiceValidator) { Style = Prompts.ListStyle.SuggestedAction };

        public Task GetDialogStep(DialogContext dialogContext, object result, SkipStepFunction next)
        {
            var state = dialogContext.Context.GetConversationState<InMemoryPromptState>();
            if (state.AppointmentType == AppointmentType.Cancel)
            { 
                return next();
            }

            var cardOptions = new ChoicePromptOptions
            {
                Choices = new List<Choice>
                {
                    new Choice
                    {
                        Value = AppointmentReason.UrgentSurgery.GetDescription(),
                        Synonyms = new List<string> { AppointmentReason.UrgentSurgery.ToString() }
                    },
                    new Choice
                    {
                        Value = AppointmentReason.MedicalExamination.GetDescription(),
                        Synonyms = new List<string> { AppointmentReason.MedicalExamination.ToString() }
                    },
                    new Choice
                    {
                        Value = AppointmentReason.NewPatient.GetDescription(),
                        Synonyms = new List<string> { AppointmentReason.NewPatient.ToString() }
                    },
                }
                
            };

            return dialogContext.Prompt(Name, "Was ist der Grund für Ihren Termin?", cardOptions);
        }

        private static async Task ChoiceValidator(ITurnContext context, Prompts.ChoiceResult result)
        {
            if (!result.Succeeded())
            {
                result.Status = Prompts.PromptStatus.NotRecognized;
                await context.SendActivity("Asuwahl nicht erkannt.");
            }

            var state = context.GetConversationState<InMemoryPromptState>();
            state.AppointmentReason = result.Value.Value.GetValueFromDescription<AppointmentReason>();
        }
    }
}
