﻿using System;
using System.Globalization;
using MedicalAppointment.App.Models;
using MedicalAppointment.Common.Storage.Interfaces;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MedicalAppointment.Common.Entities;
using Microsoft.Bot.Builder.Dialogs.Choices;

namespace MedicalAppointment.App.Bots.Dialogs
{
    internal class AppointmentDatesToCancelDialog : IPromptDialog
    {
        private const string DialogId = nameof(AppointmentDatesToCancelDialog);
        private readonly IPatientStorage _patientStorage;
        private readonly BotStateAccessors _accessors;

        public AppointmentDatesToCancelDialog(IPatientStorage patientStorage, BotStateAccessors accessor)
        {
            _patientStorage = patientStorage;
            _accessors = accessor;
        }

        
        public Dialog GetDialog() => new ChoicePrompt(DialogId, ChoiceValidator);
        
        public async Task<DialogTurnResult> GetWaterfallStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var userProfile = await _accessors.UserProfile.GetAsync(stepContext.Context, () => new UserProfile(), cancellationToken);

            if (userProfile.AppointmentType == AppointmentType.Create)
            {
                return await stepContext.NextAsync(cancellationToken: cancellationToken);
            }

            var patient = await _patientStorage.Get(userProfile.FirstName, userProfile.LastName, userProfile.Birthdate);

            if (patient == null || patient.Appointments.Count == 0)
            {
                var reply = stepContext.Context.Activity.CreateReply("Wir haben leider keine Termine für Sie gefunden.");
                await stepContext.Context.SendActivityAsync(reply, cancellationToken);
                return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
            }

            var options = await GetOptions(userProfile, patient);

            await _accessors.UserState.SaveChangesAsync(stepContext.Context, cancellationToken: cancellationToken);
            return await stepContext.PromptAsync(DialogId, options, cancellationToken);
        }
        
        private async Task<bool> ChoiceValidator(PromptValidatorContext<FoundChoice> promptContext, CancellationToken cancellationToken)
        {
            if (!promptContext.Recognized.Succeeded)
            {
                return await Task.FromResult(false);
            }

            var userProfile = await _accessors.UserProfile.GetAsync(promptContext.Context, () => new UserProfile(), cancellationToken);
            userProfile.AppointmentDate = DateTime.Parse(promptContext.Recognized.Value.Value);
            await _accessors.UserState.SaveChangesAsync(promptContext.Context, cancellationToken: cancellationToken);

            return await Task.FromResult(true);
        }

        private async Task<PromptOptions> GetOptions(UserProfile userProfile, Patient patient)
        {
            userProfile.PatientId = patient.Id;

            return new PromptOptions
            {
                Choices = ChoiceFactory.ToChoices(patient.Appointments.Select(a => a.AppointmentStart.ToString()).ToList()),
                Prompt = MessageFactory.Text("Wählen Sie den Termin aus, den sie absagen möchten."),
                RetryPrompt = MessageFactory.Text("Die Auswahl wurde nicht erkannt, bitte versuchen Sie es erneut.")
            };
        }
    }
}
