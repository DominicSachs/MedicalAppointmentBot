using System.Collections.Generic;
using System.Threading.Tasks;
using MedicalAppointment.App.Models;
using MedicalAppointment.Common.Storage.Interfaces;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Prompts.Choices;
using Microsoft.Recognizers.Text;
using Prompts = Microsoft.Bot.Builder.Prompts;

namespace MedicalAppointment.App.Bots.Dialogs
{
    internal class AppointmentDatesToCancelDialog : IPromptDialog
    {
        private readonly IPatientStorage _patientStorage;

        public AppointmentDatesToCancelDialog(IPatientStorage patientStorage)
        {
            _patientStorage = patientStorage;
        }

        public string Name => nameof(AppointmentDatesToCancelDialog);

        public IDialog GetDialog() => new ChoicePrompt(Culture.German, ChoiceValidator);

        public Task GetDialogStep(DialogContext dialogContext, object result, SkipStepFunction next)
        {
            var state = dialogContext.Context.GetConversationState<InMemoryPromptState>();
            _patientStorage.Get(state.FirstName, state.LastName, state.Birthdate);


            var cardOptions = new ChoicePromptOptions
            {
                Choices = new List<Choice>
                {
                    new Choice
                    {
                        Value = "30.12.2018",
                        Synonyms = new List<string> { "30.12.2018" }
                    },
                    new Choice
                    {
                        Value = "30.01.2019",
                        Synonyms = new List<string> { "30.01.2019" }
                    },
                    new Choice
                    {
                        Value = "alle",
                        Synonyms = new List<string> { "alle" }
                    }
                }
            };

            return dialogContext.Prompt(Name, "Wählen Sie den Termin aus, den sie absagen möchten?", cardOptions);
        }

        private static async Task ChoiceValidator(ITurnContext context, Prompts.ChoiceResult result)
        {
            if (!result.Succeeded())
            {
                result.Status = Prompts.PromptStatus.NotRecognized;
                await context.SendActivity("Asuwahl nicht erkannt.");
            }
            var value = result.Value;
        }
    }
}
