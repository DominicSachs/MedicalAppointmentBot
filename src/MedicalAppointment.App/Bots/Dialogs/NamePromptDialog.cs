using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using PromptsDialog = Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Prompts;

namespace MedicalAppointment.App.Bots.Dialogs
{
    internal class NamePromptDialog : IPromptDialog
    {
        public string Name => "NamePrompt";

        public PromptsDialog.IDialog GetDialog()
        {
            return new PromptsDialog.TextPrompt(NameValidator);
        }

        public async Task GetDialogStep(PromptsDialog.DialogContext dialogContext, object result, PromptsDialog.SkipStepFunction next)
        {
            await dialogContext.Prompt(Name, "What is your full name?");
        }

        private static async Task NameValidator(ITurnContext context, TextResult result)
        {
            var names = result.Value?.Split(' ');
            if (names?.Length < 2)
            {
                result.Status = PromptStatus.NotRecognized;
                await context.SendActivity("Your name should be at least 2 words.");
            }
        }
    }
}
