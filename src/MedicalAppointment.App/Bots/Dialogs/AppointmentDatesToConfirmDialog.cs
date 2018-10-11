using System;
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
    internal class AppointmentDatesToConfirmDialog : IPromptDialog
    {
        private const string DialogId = nameof(AppointmentDatesToConfirmDialog);
        private readonly IPatientStorage _patientStorage;
        private readonly IAppointmentStorage _appointmentStorage;
        private readonly BotStateAccessors _accessors;

        public AppointmentDatesToConfirmDialog(IPatientStorage patientStorage, IAppointmentStorage appointmentStorage, BotStateAccessors accessors)
        {
            _patientStorage = patientStorage;
            _appointmentStorage = appointmentStorage;
            _accessors = accessors;
        }

        public Dialog GetDialog() => new ChoicePrompt(DialogId, ChoiceValidator);

        public async Task<DialogTurnResult> GetWaterfallStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var userProfile = await _accessors.UserProfile.GetAsync(stepContext.Context, () => new UserProfile(), cancellationToken);

            if (userProfile.AppointmentType == AppointmentType.Cancel)
            {
                return await stepContext.NextAsync(cancellationToken: cancellationToken);
            }

            var freeAppointments = await _appointmentStorage.GetFreeAppointments();

            return await stepContext.PromptAsync(DialogId, new PromptOptions
            {
                Choices = ChoiceFactory.ToChoices(freeAppointments.Select(a => a.AppointmentStart.ToString()).ToList()),
                Prompt = MessageFactory.Text("Bitte wählen Sie einen Termin aus."),
                RetryPrompt = MessageFactory.Text("Die Auswahl wurde nicht erkannt, bitte versuchen Sie es erneut.")
            }, cancellationToken);
        }
        
        private async Task<bool> ChoiceValidator(PromptValidatorContext<FoundChoice> promptContext, CancellationToken cancellationToken)
        {
            if (!promptContext.Recognized.Succeeded)
            {
                return await Task.FromResult(false);
            }

            var userProfile = await _accessors.UserProfile.GetAsync(promptContext.Context, () => new UserProfile(), cancellationToken);
            userProfile.AppointmentDate = DateTime.Parse(promptContext.Recognized.Value.Value);
            await _accessors.UserState.SaveChangesAsync(promptContext.Context, cancellationToken: cancellationToken);

            return await Task.FromResult(true);
        }
    }
}
