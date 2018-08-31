﻿using System.Collections.Generic;
using System.Threading.Tasks;
using MedicalAppointment.App.Bots.Dialogs;
using MedicalAppointment.App.Models;
using MedicalAppointment.Common.Storage.Interfaces;
using Microsoft.Bot;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Prompts;
using Microsoft.Bot.Schema;

namespace MedicalAppointment.App.Bots
{
    public class AppointmentBot : IBot
    {
        private readonly DialogSet _dialogs;

        public AppointmentBot(IDialogFactory dialogFactory, IPatientStorage patientStorage)
        {
            _dialogs = new DialogSet();
            var waterFallSteps = new List<WaterfallStep>();

            foreach (var dialog in dialogFactory.GetDialogs(patientStorage))
            {
                _dialogs.Add(dialog.Name, dialog.GetDialog());
                waterFallSteps.Add(dialog.GetDialogStep);
            }

            waterFallSteps.Add(GatherInfoStep);
            _dialogs.Add("GatherInfo", waterFallSteps.ToArray());
        }

        public async Task OnTurn(ITurnContext context)
        {
            var state = context.GetConversationState<InMemoryPromptState>();
            var dialogCtx = _dialogs.CreateContext(context, state);

            switch (context.Activity.Type)
            {
                case ActivityTypes.Message:
                    await dialogCtx.Continue();
                    if (!context.Responded)
                    {
                        await dialogCtx.Begin("GatherInfo");
                    }
                    break;
            }
        }

        private async Task GatherInfoStep(DialogContext dialogContext, object result, SkipStepFunction next)
        {
            var state = dialogContext.Context.GetConversationState<InMemoryPromptState>();
            //await dialogContext.Context.SendActivity($"Your name is {state.Name} and your age is {state.Age}");
            await dialogContext.End();
        }

    }
}
