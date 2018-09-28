using MedicalAppointment.App.Models;
using MedicalAppointment.Common.Storage.Interfaces;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs.Choices;

namespace MedicalAppointment.App.Bots.Dialogs
{
    internal class AppointmentDatesToCancelDialog : IPromptDialog
    {
        private const string DialogId = nameof(AppointmentDatesToCancelDialog);
        private readonly IPatientStorage _patientStorage;
        private readonly BotStateAccessors _accessors;

        public AppointmentDatesToCancelDialog(IPatientStorage patientStorage, BotStateAccessors accessor)
        {
            _patientStorage = patientStorage;
            _accessors = accessor;
        }

        
        public Dialog GetDialog() => new ChoicePrompt(DialogId, ChoiceValidator);
        
        public async Task<DialogTurnResult> GetWaterfallStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var userProfile = await _accessors.UserProfile.GetAsync(stepContext.Context, () => new UserProfile(), cancellationToken);

            if (userProfile.AppointmentType == AppointmentType.Create)
            {
                return await stepContext.NextAsync(cancellationToken: cancellationToken);
            }

            var options = await GetOptions(stepContext, cancellationToken);
            return await stepContext.PromptAsync(DialogId, options, cancellationToken);
        }
        
        private async Task<bool> ChoiceValidator(PromptValidatorContext<FoundChoice> promptContext, CancellationToken cancellationToken)
        {
            if (!promptContext.Recognized.Succeeded)
            {
                return await Task.FromResult(false);
            }

            return await Task.FromResult(true);
        }

        private async Task<PromptOptions> GetOptions(WaterfallStepContext context, CancellationToken cancellationToken)
        {
            var userProfile = await _accessors.UserProfile.GetAsync(context.Context, () => new UserProfile(), cancellationToken);

            var patient = await _patientStorage.Get(userProfile.FirstName, userProfile.LastName, userProfile.Birthdate);
            userProfile.PatientId = patient.Id;
            await _accessors.UserState.SaveChangesAsync(context.Context, cancellationToken: cancellationToken);

            return new PromptOptions
            {
                Choices = patient.Appointments.Select(a => new Choice { Value = a.AppointmentStart.ToString() }).ToList(),
                Prompt = MessageFactory.Text("Wählen Sie den Termin aus, den sie absagen möchten."),
                RetryPrompt = MessageFactory.Text("Auswahl nicht erkannt.")
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
