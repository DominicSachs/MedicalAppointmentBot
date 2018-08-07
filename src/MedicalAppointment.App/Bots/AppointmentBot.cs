using System.Threading.Tasks;
using MedicalAppointment.App.Bots.Dialogs;
using MedicalAppointment.App.Models;
using Microsoft.Bot;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Prompts;
using Microsoft.Bot.Schema;
using Microsoft.Recognizers.Text;

namespace MedicalAppointment.App.Bots
{
    public class AppointmentBot : IBot
    {
        private readonly DialogSet _dialogs;

        public AppointmentBot()
        {
            _dialogs = new DialogSet();

            _dialogs.Add(ChoicePromptDialog.Id, ChoicePromptDialog.GetPromptDialog());
            _dialogs.Add(NamePromptDialog.Id, NamePromptDialog.GetPromptDialog());
            _dialogs.Add(BithdatePromptDialog.Id, BithdatePromptDialog.GetPromptDialog());

            _dialogs.Add("GatherInfo", new WaterfallStep[]
            {
                ChoicePromptDialog.GetCardStep, 
                NamePromptDialog.GetCardStep, 
                BithdatePromptDialog.GetCardStep,
                GatherInfoStep
            });
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
            var state_Birthdate = (result as DateTimeResult);
            //await dialogContext.Context.SendActivity($"Your name is {state.Name} and your age is {state.Age}");
            await dialogContext.End();
        }

    }
}
