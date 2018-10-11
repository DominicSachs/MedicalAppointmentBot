using MedicalAppointment.App.Models;
using MedicalAppointment.Common.Extensions;
using MedicalAppointment.Common.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs.Choices;

namespace MedicalAppointment.App.Bots.Dialogs
{
    internal class AppointmentReasonPromptDialog : IPromptDialog
    {
        private string DialogId = nameof(AppointmentReasonPromptDialog);
        private readonly BotStateAccessors _accessors;

        public AppointmentReasonPromptDialog(BotStateAccessors accessors)
        {
            _accessors = accessors;
        }

        public Dialog GetDialog() => new ChoicePrompt(DialogId, ChoiceValidator) { Style = ListStyle.SuggestedAction };

        public async Task<DialogTurnResult> GetWaterfallStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var userProfile = await _accessors.UserProfile.GetAsync(stepContext.Context, () => new UserProfile(), cancellationToken);
            
            if (userProfile.AppointmentType == AppointmentType.Cancel)
            {
                return await stepContext.NextAsync(cancellationToken: cancellationToken);
            }

            return await stepContext.PromptAsync(DialogId, GetOptions(), cancellationToken);
        }

        private async Task<bool> ChoiceValidator(PromptValidatorContext<FoundChoice> promptContext, CancellationToken cancellationToken)
        {
            if (!promptContext.Recognized.Succeeded)
            {
                await promptContext.Context.SendActivityAsync("Asuwahl nicht erkannt.", cancellationToken: cancellationToken);
                return await Task.FromResult(false);
            }

            var userProfile = await _accessors.UserProfile.GetAsync(promptContext.Context, () => new UserProfile(), cancellationToken);
            userProfile.AppointmentReason = promptContext.Recognized.Value.Value.GetValueFromDescription<AppointmentReason>();
            return await Task.FromResult(true);
        }

        private static PromptOptions GetOptions()
        {
            var choices = new List<string>
            {
                AppointmentReason.UrgentSurgery.GetDescription(),
                AppointmentReason.MedicalExamination.GetDescription(),
                AppointmentReason.NewPatient.GetDescription()
            };
            
            return new PromptOptions
            {
                Choices = ChoiceFactory.ToChoices(choices),
                Prompt = MessageFactory.Text("Was ist der Anlass Ihres Termins?")
            };
        }
    }
}
