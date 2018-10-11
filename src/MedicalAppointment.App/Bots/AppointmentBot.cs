using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MedicalAppointment.App.Bots.Dialogs;
using MedicalAppointment.Common.Storage.Interfaces;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;


namespace MedicalAppointment.App.Bots
{
    public class AppointmentBot : IBot
    {
        private const string WelcomeText = "Hallo, ich bin der Termin Assistent. Ich leite Sie durch den Prozess."; 
        private readonly DialogSet _dialogs;
        private readonly BotStateAccessors _accessors;

        public AppointmentBot(BotStateAccessors accessors, IDialogFactory dialogFactory, IPatientStorage patientStorage, IAppointmentStorage appointmentStorage)
        {
            _accessors = accessors ?? throw new ArgumentNullException(nameof(accessors));
            _dialogs = new DialogSet(_accessors.ConversationDialogState);

            var waterFallSteps = new List<WaterfallStep>();

            var dialogs = dialogFactory.GetDialogs(patientStorage, appointmentStorage, accessors);

            foreach (var dialog in dialogs)
            {
                waterFallSteps.Add(dialog.GetWaterfallStepAsync);
            }

            _dialogs.Add(new WaterfallDialog("details", waterFallSteps));

            foreach (var dialog in dialogs)
            {
                _dialogs.Add(dialog.GetDialog());
            }
        }

        public async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (turnContext == null)
            {
                throw new ArgumentNullException(nameof(turnContext));
            }

            if (turnContext.Activity.Type == ActivityTypes.Message)
            {
                // Run the DialogSet - let the framework identify the current state of the dialog from
                // the dialog stack and figure out what (if any) is the active dialog.
                var dialogContext = await _dialogs.CreateContextAsync(turnContext, cancellationToken);
                var results = await dialogContext.ContinueDialogAsync(cancellationToken);

                if (results.Status == DialogTurnStatus.Empty)
                {
                    await dialogContext.BeginDialogAsync("details", null, cancellationToken);
                }
            }
            else if (turnContext.Activity.Type == ActivityTypes.ConversationUpdate)
            {
                if (turnContext.Activity.MembersAdded.Any())
                {
                    await SendWelcomeMessageAsync(turnContext, cancellationToken);

                    var dialogContext = await _dialogs.CreateContextAsync(turnContext, cancellationToken);
                    var results = await dialogContext.ContinueDialogAsync(cancellationToken);
                    if (results.Status == DialogTurnStatus.Empty)
                    {
                        await dialogContext.BeginDialogAsync("details", null, cancellationToken);
                    }
                }
            }
            else
            {
                await turnContext.SendActivityAsync($"{turnContext.Activity.Type} event detected", cancellationToken: cancellationToken);
            }

            await _accessors.ConversationState.SaveChangesAsync(turnContext, false, cancellationToken);
            await _accessors.UserState.SaveChangesAsync(turnContext, false, cancellationToken);
        }

        private static async Task SendWelcomeMessageAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in turnContext.Activity.MembersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(WelcomeText, cancellationToken: cancellationToken);
                }
            }
        }
    }
}
