using System.Collections.Generic;

namespace MedicalAppointment.App.Bots.Dialogs
{
    public class DialogFactory : IDialogFactory
    {
        public IEnumerable<IPromptDialog> GetDialogs()
        {
            yield return new ChoicePromptDialog();
            yield return new NamePromptDialog();
            yield return new BithdatePromptDialog();
        }
    }
}
