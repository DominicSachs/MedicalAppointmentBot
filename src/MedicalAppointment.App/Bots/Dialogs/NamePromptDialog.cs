using System.Linq;
using System.Threading.Tasks;
using MedicalAppointment.App.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Builder.Dialogs;
using Prompts = Microsoft.Bot.Builder.Prompts;

namespace MedicalAppointment.App.Bots.Dialogs
{
    internal class NamePromptDialog : IPromptDialog
    {
        public string Name => nameof(NamePromptDialog);

        public IDialog GetDialog() => new TextPrompt(NameValidator);

        public async Task GetDialogStep(DialogContext dialogContext, object result, SkipStepFunction next)
        {
            await dialogContext.Prompt(Name, "Wie ist Ihr voller Name?");
        }

        private static async Task NameValidator(ITurnContext context, Prompts.TextResult result)
        {
            var names = result.Value?.Split(' ');
            if (names?.Length < 2)
            {
                result.Status = Prompts.PromptStatus.NotRecognized;
                await context.SendActivity("Ihr Name muss mindestens aus zwei Wörter bestehen.");
            }
            
            var state = context.GetConversationState<InMemoryPromptState>();
            state.FirstName = names[0];
            state.LastName = string.Join(' ', names.Skip(1));
        }
    }
}
