using MedicalAppointment.App.Models;
using MedicalAppointment.Common.Storage.Interfaces;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Prompts.Choices;
using Microsoft.Recognizers.Text;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task GetDialogStep(DialogContext dialogContext, object result, SkipStepFunction next)
        {
            var state = dialogContext.Context.GetConversationState<InMemoryPromptState>();

            if (state.AppointmentType == AppointmentType.Create)
            {
                await next();
            }

            var cardOptions = await GetOptions(dialogContext);

            await dialogContext.Prompt(Name, "Wählen Sie den Termin aus, den sie absagen möchten?", cardOptions);
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

        private async Task<ChoicePromptOptions> GetOptions(DialogContext context)
        {
            var state = context.Context.GetConversationState<InMemoryPromptState>();
            var patient = await _patientStorage.Get(state.FirstName, state.LastName, state.Birthdate);

            return new ChoicePromptOptions
            {
                Choices = patient.Appointments.Select(a => new Choice { Value = a.AppointmentStart.ToString() }).ToList()
                //Choices = new List<Choice>
                //{
                //    new Choice
                //    {
                //        Value = "alle",
                //        Synonyms = new List<string> { "alle" }
                //    }
                //}
            };
        }
    }
}
