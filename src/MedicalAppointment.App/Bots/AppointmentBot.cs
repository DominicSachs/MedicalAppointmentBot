using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MedicalAppointment.App.Models;
using Microsoft.Bot;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Prompts;
using Microsoft.Bot.Builder.Prompts.Choices;
using Microsoft.Bot.Schema;
using Microsoft.Recognizers.Text;
using PromptsDialog = Microsoft.Bot.Builder.Dialogs;
using Microsoft.Recognizers.Text;

namespace MedicalAppointment.App.Bots
{
    public class AppointmentBot : IBot
    {
        private readonly DialogSet _dialogs;

        public AppointmentBot()
        {
            _dialogs = new DialogSet();

            _dialogs.Add("ChoicePrompt", new PromptsDialog.ChoicePrompt(Culture.German, AppointmentChoiceValidator));
            _dialogs.Add("NamePrompt", new PromptsDialog.TextPrompt(NameValidator));
            _dialogs.Add("DatePrompt", new PromptsDialog.DateTimePrompt(Culture.German, DateValidator));


            _dialogs.Add("GatherInfo", new WaterfallStep[] { ChoiceCardStep, AskNameStep, AskAgeStep, GatherInfoStep });
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

        private async Task AskNameStep(DialogContext dialogContext, object result, SkipStepFunction next)
        {
            await dialogContext.Prompt("NamePrompt", "Wie ist Ihr voller Name?");
        }

        private async Task AskAgeStep(DialogContext dialogContext, object result, SkipStepFunction next)
        {
            var state = dialogContext.Context.GetConversationState<InMemoryPromptState>();
            state.Name = (result as TextResult)?.Value;
            await dialogContext.Prompt("DatePrompt", "Wie ist Ihr Geburtsdatum?");
        }

        private async Task GatherInfoStep(DialogContext dialogContext, object result, SkipStepFunction next)
        {
            var state = dialogContext.Context.GetConversationState<InMemoryPromptState>();
            var state_Birthdate = (result as DateTimeResult);
            //await dialogContext.Context.SendActivity($"Your name is {state.Name} and your age is {state.Age}");
            await dialogContext.End();
        }
        private Task ChoiceCardStep(DialogContext dialogContext, object result, SkipStepFunction next)
        {
            var cardOptions = new ChoicePromptOptions
            {
                Choices = new List<Choice>
                {
                    new Choice
                    {
                        Value = "Terminanfrage",
                        Synonyms = new List<string> { AppointmentType.Create.ToString() }
                    },
                    new Choice
                    {
                        Value = "Terminabsage",
                        Synonyms = new List<string> { AppointmentType.Cancel.ToString() }
                    }
                }
            };
            return dialogContext.Prompt("ChoicePrompt", "Was möchten Sie tun?", cardOptions);
        }

        private async Task AppointmentChoiceValidator(ITurnContext context, ChoiceResult result)
        {
            if(!result.Succeeded())
            {
                result.Status = PromptStatus.NotRecognized;
                await context.SendActivity("Asuwahl nicht erkannt.");
            }
            var value = result.Value;
        }

        private async Task NameValidator(ITurnContext context, TextResult result)
        {
            var names = result.Value?.Split(' ');
            if (names?.Length < 2)
            {
                result.Status = PromptStatus.NotRecognized;
                await context.SendActivity("Your name should be at least 2 characters long.");
            }
        }

        private async Task DateValidator(ITurnContext context, DateTimeResult result)
        {
            if (!result.Succeeded())
            {
                result.Status = PromptStatus.NotRecognized;
                await context.SendActivity("Sie haben kein gültiges Datum angegeben.");
            }
        }
    }
}
