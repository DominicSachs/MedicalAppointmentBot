using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MedicalAppointment.App.Bots.Dialogs;
using MedicalAppointment.App.Models;
using MedicalAppointment.Common.Storage.Interfaces;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;


namespace MedicalAppointment.App.Bots
{
    public class AppointmentBot : IBot
    {
        private readonly DialogSet _dialogs;
        private readonly BotStateAccessors _accessors;

        public AppointmentBot(BotStateAccessors accessors, IDialogFactory dialogFactory, IPatientStorage patientStorage)
        {
            _accessors = accessors ?? throw new ArgumentNullException(nameof(accessors));
            _dialogs = new DialogSet(_accessors.ConversationDialogState);

            var waterFallSteps = new List<WaterfallStep>();

            var dialogs = dialogFactory.GetDialogs(patientStorage, accessors);
            //var steps = dialogs.Select<WaterfallStep>(d => d.GetWaterfallStepAsync);

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

                // If the DialogTurnStatus is Empty we should start a new dialog.
                if (results.Status == DialogTurnStatus.Empty)
                {
                    await dialogContext.BeginDialogAsync("details", null, cancellationToken);
                }
            }
            else if (turnContext.Activity.Type == ActivityTypes.ConversationUpdate)
            {
                //if (turnContext.Activity.MembersAdded.Any())
                //{
                //    await SendWelcomeMessageAsync(turnContext, cancellationToken);
                //}
            }
            else
            {
                await turnContext.SendActivityAsync($"{turnContext.Activity.Type} event detected", cancellationToken: cancellationToken);
            }

            // Save the dialog state into the conversation state.
            await _accessors.ConversationState.SaveChangesAsync(turnContext, false, cancellationToken);

            // Save the user profile updates into the user state.
            await _accessors.UserState.SaveChangesAsync(turnContext, false, cancellationToken);
            //var state = context.GetConversationState<InMemoryPromptState>();
            //var dialogCtx = _dialogs.CreateContext(context, state);

            //switch (context.Activity.Type)
            //{
            //    // If Activity has an ActivityType of ConversationUpdate,
            //    // this bot will send a greeting message to users joining the conversation.
            //    //case ActivityTypes.ConversationUpdate:
            //    //    //If members were added, send a welcome message to every member added that is not the bot.
            //    //    if (context.Activity.MembersAdded.Any())
            //    //    {
            //    //        foreach (var member in context.Activity.MembersAdded)
            //    //        {
            //    //            var newUserName = member.Name;

            //    //            if (member.Id != context.Activity.Recipient.Id)
            //    //            {
            //    //                context.SendActivity($"Hello {newUserName}!");
            //    //            }
            //    //        }
            //    //    }

            //    //    break;
            //    case ActivityTypes.Message:
            //        await dialogCtx.Continue();
            //        if (!context.Responded)
            //        {
            //            await dialogCtx.Begin("GatherInfo");
            //        }
            //        break;
            //}
        }

        private async Task<DialogTurnResult> GatherInfoStep(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var userProfile = await _accessors.UserProfile.GetAsync(stepContext.Context, () => new UserProfile(), cancellationToken);
            await stepContext.Context.SendActivityAsync(MessageFactory.Text($"I have your name as {userProfile.FirstName} {userProfile.LastName}."), cancellationToken);

            return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
        }
    }
}
