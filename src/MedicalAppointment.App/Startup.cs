using System;
using System.Linq;
using MedicalAppointment.App.Bots;
using MedicalAppointment.App.Bots.Dialogs;
using MedicalAppointment.App.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder.BotFramework;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MedicalAppointment.Common.Extensions;
using MedicalAppointment.Common.Storage.Implementations;
using MedicalAppointment.Common.Storage.Interfaces;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Integration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MedicalAppointment.App
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private ILoggerFactory _loggerFactory;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(_configuration);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.ConfigureDataStorage(_configuration);

            services.AddSingleton<IDialogFactory, DialogFactory>();
            services.AddTransient<IPatientStorage, PatientStorage>();

            services.AddBot<AppointmentBot>(options =>
            {
                options.CredentialProvider = new ConfigurationCredentialProvider(_configuration);
                ILogger logger = _loggerFactory.CreateLogger<AppointmentBot>();

                options.OnTurnError = async (context, exception) =>
                {
                    logger.LogError($"Exception caught : {exception}");
                    await context.SendActivityAsync("Sorry, it looks like something went wrong.");
                };

                IStorage dataStore = new MemoryStorage();
                var conversationState = new ConversationState(dataStore);
                options.State.Add(conversationState);

                var userState = new UserState(dataStore);
                options.State.Add(userState);
            });

            services.AddSingleton(sp =>
            {
                var options = sp.GetRequiredService<IOptions<BotFrameworkOptions>>().Value;
                if (options == null)
                {
                    throw new InvalidOperationException("BotFrameworkOptions must be configured prior to setting up the State Accessors");
                }

                var conversationState = options.State.OfType<ConversationState>().FirstOrDefault();
                if (conversationState == null)
                {
                    throw new InvalidOperationException("ConversationState must be defined and added before adding conversation-scoped state accessors.");
                }

                var userState = options.State.OfType<UserState>().FirstOrDefault();
                if (userState == null)
                {
                    throw new InvalidOperationException("UserState must be defined and added before adding user-scoped state accessors.");
                }

                var accessors = new BotStateAccessors(conversationState, userState)
                {
                    ConversationDialogState = conversationState.CreateProperty<DialogState>("DialogState"),
                    UserProfile = userState.CreateProperty<UserProfile>("UserProfile"),
                };

                return accessors;
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles()
                .UseStaticFiles()
                .UseBotFramework();
        }
    }
}
