using System.Linq;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using System.Threading;
using System.Threading.Tasks;
using MedicalAppointment.App.Models;

namespace MedicalAppointment.App.Bots.Dialogs
{
    internal class NamePromptDialog : IPromptDialog
    {
        private const string DialogId = nameof(NamePromptDialog);
        private readonly BotStateAccessors _accessors;

        public NamePromptDialog(BotStateAccessors accessors)
        {
            _accessors = accessors;
        }

        public Dialog GetDialog()
        {
            return new TextPrompt(DialogId, NameValidator); 
        }

        public async Task<DialogTurnResult> GetWaterfallStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        { 
            return await stepContext.PromptAsync(DialogId,
                new PromptOptions
                {
                    Prompt = MessageFactory.Text("Bitte geben Sie ihren vollständigen Namen ein."),
                    RetryPrompt = MessageFactory.Text("Ihr Name muss mindestens aus zwei Wörtern bestehen. Bitte versuchen Sie es erneut."),
                }, cancellationToken);
        }


        private async Task<bool> NameValidator(PromptValidatorContext<string> promptContext, CancellationToken cancellationToken)
        {
            var names = promptContext.Recognized.Value?.Split(' ');

            if (names?.Length < 2)
            {
                return await Task.FromResult(false);
            }

            var userProfile = await _accessors.UserProfile.GetAsync(promptContext.Context, () => new UserProfile(), cancellationToken);
            userProfile.FirstName = names[0];
            userProfile.LastName = string.Join(' ', names.Skip(1));

            return await Task.FromResult(true);
        }
    }
}
