using MedicalAppointment.App.Models;
using MedicalAppointment.Common.Storage.Interfaces;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs.Choices;

namespace MedicalAppointment.App.Bots.Dialogs
{
    internal class AppointmentDatesToConfirmDialog : IPromptDialog
    {
        private const string DialogId = nameof(AppointmentDatesToConfirmDialog);
        private readonly IPatientStorage _patientStorage;
        private readonly BotStateAccessors _accessors;

        public AppointmentDatesToConfirmDialog(IPatientStorage patientStorage, BotStateAccessors accessors)
        {
            _patientStorage = patientStorage;
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

            return await stepContext.PromptAsync(DialogId, new PromptOptions
            {
                Prompt = MessageFactory.Text("Bitte wählen Sie ein Datum aus."),
                RetryPrompt = MessageFactory.Text("Asuwahl nicht erkannt."),
                Choices = new List<Choice>
                {
                    new Choice
                    {
                        Value = "30.12.2018",
                        Synonyms = new List<string> { "30.12.2018 09:00" }
                    },
                    new Choice
                    {
                        Value = "30.01.2019",
                        Synonyms = new List<string> { "30.01.2019 11:00" }
                    }
                }
            }, cancellationToken);
        }
        
        private async Task<bool> ChoiceValidator(PromptValidatorContext<FoundChoice> promptContext, CancellationToken cancellationToken)
        {
            if (!promptContext.Recognized.Succeeded)
            {
                return await Task.FromResult(false);
            }

            return await Task.FromResult(true);
        }
    }
}
