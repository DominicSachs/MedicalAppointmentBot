using System;
using System.Collections.Generic;
using System.IO;
using MedicalAppointment.App.Models;
using Microsoft.Bot.Builder.Dialogs;
using System.Threading;
using System.Threading.Tasks;
using MedicalAppointment.Common.Extensions;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;

namespace MedicalAppointment.App.Bots.Dialogs
{
    internal class AppointmentConfirmDialog : IPromptDialog
    {
        private const string DialogId = nameof(AppointmentConfirmDialog);
        private readonly BotStateAccessors _accessors;

        public AppointmentConfirmDialog(BotStateAccessors accessors)
        {
            _accessors = accessors;
        }

        public Dialog GetDialog() => new ChoicePrompt(DialogId, ChoiceValidator);

        public async Task<DialogTurnResult> GetWaterfallStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var userProfile = await _accessors.UserProfile.GetAsync(stepContext.Context, () => new UserProfile(), cancellationToken);

            if (userProfile.AppointmentType == AppointmentType.Cancel)
            {
                var reply = stepContext.Context.Activity.CreateReply();
                reply.Attachments.Add(GetCancelHeroCard(userProfile).ToAttachment());
                await stepContext.Context.SendActivityAsync(reply, cancellationToken);
            }
            else
            {
                var reply = stepContext.Context.Activity.CreateReply();
                reply.Attachments.Add(GetConfirmHeroCard(userProfile).ToAttachment());
                await stepContext.Context.SendActivityAsync(reply, cancellationToken);
            }

            return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
        }
        
        private async Task<bool> ChoiceValidator(PromptValidatorContext<FoundChoice> promptContext, CancellationToken cancellationToken)
        {
            if (!promptContext.Recognized.Succeeded)
            {
                return await Task.FromResult(false);
            }

            return await Task.FromResult(true);
        }

        private static HeroCard GetCancelHeroCard(UserProfile userProfile)
        {
            var imageData = File.ReadAllBytes(@".\Images\appointment.jpeg");
            var image64 = "data:image/jpeg;base64," + Convert.ToBase64String(imageData);
            
            var heroCard = new HeroCard
            {
                Title = userProfile.AppointmentType.GetDescription(),
                Subtitle = $"{userProfile.FirstName} {userProfile.LastName}",
                Text = $"Ihr Termin am {userProfile.AppointmentDate.ToLongDateString()} wurde erfolgreich gelöscht.",
                Images = new List<CardImage> { new CardImage(image64) }
            };

            return heroCard;
        }

        private static HeroCard GetConfirmHeroCard(UserProfile userProfile)
        {
            var imageData = File.ReadAllBytes(@".\Images\appointment.jpeg");
            var image64 = "data:image/jpeg;base64," + Convert.ToBase64String(imageData);

            var heroCard = new HeroCard
            {
                Title = userProfile.AppointmentType.GetDescription(),
                Subtitle = $"{userProfile.FirstName} {userProfile.LastName}",
                Text = $@"Ihr Termin ({userProfile.AppointmentReason.GetDescription()}) am {userProfile.AppointmentDate.ToLongDateString()} ist hiermit bestätigt.
                          {Environment.NewLine}Bitte bringen Sie zu diesem Termin Ihre Versicherungskarte mit.",
                Images = new List<CardImage> { new CardImage(image64) }
            };

            return heroCard;
        }
    }
}
