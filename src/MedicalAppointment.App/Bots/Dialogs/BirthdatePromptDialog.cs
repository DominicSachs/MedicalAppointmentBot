using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MedicalAppointment.App.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;

namespace MedicalAppointment.App.Bots.Dialogs
{
    internal class BithdatePromptDialog : IPromptDialog
    {
        private const string DialogId = nameof(BithdatePromptDialog);
        private readonly BotStateAccessors _accessors;

        public BithdatePromptDialog(BotStateAccessors accessors)
        {
            _accessors = accessors;
        }

        public Dialog GetDialog() => new DateTimePrompt(DialogId, DateValidator);

        public async Task<DialogTurnResult> GetWaterfallStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.PromptAsync(DialogId,
                new PromptOptions
                {
                    Prompt = MessageFactory.Text("Wie ist Ihr Geburtsdatum?"),
                    RetryPrompt = MessageFactory.Text("Sie haben kein gültiges Datum angegeben. Bitte versuchen Sie es erneut."),
                }, cancellationToken);
        }

        private async Task<bool> DateValidator(PromptValidatorContext<IList<DateTimeResolution>> promptContext, CancellationToken cancellationToken)
        {
            if (!promptContext.Recognized.Succeeded)
            {
                return await  Task.FromResult(false);
            }

            var birthDate = default(DateTime);
            var resolution = promptContext.Recognized.Value.FirstOrDefault(res => DateTime.TryParse(res.Value, out birthDate) && birthDate < DateTime.Now.Date);
            
            if (resolution != null)
            {
                var userProfile = await _accessors.UserProfile.GetAsync(promptContext.Context, () => new UserProfile(), cancellationToken);
                userProfile.Birthdate = birthDate;

                //// If found, keep only that result.
                //promptContext.Recognized.Value.Clear();
                //promptContext.Recognized.Value.Add(resolution);
                return await Task.FromResult(true);
            }

            return await Task.FromResult(true);
        }
    }
}
