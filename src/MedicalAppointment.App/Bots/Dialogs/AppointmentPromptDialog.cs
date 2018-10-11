using MedicalAppointment.App.Models;
using MedicalAppointment.Common.Extensions;
using Microsoft.Bot.Builder.Dialogs;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs.Choices;

namespace MedicalAppointment.App.Bots.Dialogs
{
    internal class AppointmentPromptDialog : IPromptDialog
    {
        private const string DialogId = nameof(AppointmentPromptDialog);
        private readonly BotStateAccessors _accessors;

        public AppointmentPromptDialog(BotStateAccessors accessors)
        {
            _accessors = accessors;
        }

        public Dialog GetDialog() => new ChoicePrompt(DialogId, ChoiceValidator) { Style = ListStyle.SuggestedAction };

        public async Task<DialogTurnResult> GetWaterfallStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.PromptAsync(DialogId, GetOptions(), cancellationToken);
        }

        private async Task<bool> ChoiceValidator(PromptValidatorContext<FoundChoice> promptContext, CancellationToken cancellationToken)
        {
            if (!promptContext.Recognized.Succeeded)
            {
                return await Task.FromResult(false);
            }

            var userProfile = await _accessors.UserProfile.GetAsync(promptContext.Context, () => new UserProfile(), cancellationToken);
            userProfile.AppointmentType = promptContext.Recognized.Value.Value.GetValueFromDescription<AppointmentType>();

            return await Task.FromResult(true);
        }

        private static PromptOptions GetOptions()
        {
            return new PromptOptions
            {
                Choices = ChoiceFactory.ToChoices(new List<string> { AppointmentType.Create.GetDescription(), AppointmentType.Cancel.GetDescription() }),
                Prompt = MessageFactory.Text("Was ist ihr Anliegen.")
            };
        }
    }
}
