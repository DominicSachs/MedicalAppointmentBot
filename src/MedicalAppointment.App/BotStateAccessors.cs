using System;
using MedicalAppointment.App.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;

namespace MedicalAppointment.App
{
    public class BotStateAccessors
    {
        public BotStateAccessors(ConversationState conversationState, UserState userState)
        {
            ConversationState = conversationState ?? throw new ArgumentNullException(nameof(conversationState));
            UserState = userState ?? throw new ArgumentNullException(nameof(userState));
        }

        public IStatePropertyAccessor<DialogState> ConversationDialogState { get; set; }

        public IStatePropertyAccessor<UserProfile> UserProfile { get; set; }

        public UserState UserState { get; }

        public ConversationState ConversationState { get; }
    }
}
