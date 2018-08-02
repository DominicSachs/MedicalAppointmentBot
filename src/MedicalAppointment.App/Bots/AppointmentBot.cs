using System;
using System.Threading.Tasks;
using MedicalAppointment.App.Models;
using Microsoft.Bot;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Prompts;
using Microsoft.Bot.Schema;
using Microsoft.Recognizers.Text;
using PromptsDialog = Microsoft.Bot.Builder.Dialogs;

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

        private async Task AppointmentChoiceValidator(ITurnContext context, ChoiceResult result)
        {

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
            var test = result.Text;
            if (result.Text == "A")
            {
                result.Status = PromptStatus.NotRecognized;
                await context.SendActivity("Your age should be between 0 and 122.");
            }
        }
    }
}
