using hsm_api.Infrastructure;
using hsm_api.Models;
using hsm_api.Models.Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace hsm_api.Domain.StartProduction
{
    public class StartProductionService
    {
        private readonly ITimerTillNextProductionStart _timer;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly StartProductionHttpMessageSender _messageSender;

        /// <summary>
        /// Service handels material production start events
        /// </summary>
        public StartProductionService(ITimerTillNextProductionStart timer,
                                      IServiceScopeFactory scopeFactory,
                                      StartProductionHttpMessageSender messageSender)
        {
            _timer = timer;
            _scopeFactory = scopeFactory;
            _messageSender = messageSender;

            SubscribeToAndStartTimer();
        }

        private void SubscribeToAndStartTimer()
        {
            _timer.TimeElapsed += NewProductionStartHandler;
            _timer.Start();
        }

        private async void NewProductionStartHandler(object sender, System.Timers.ElapsedEventArgs e)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                await GetSubsAndPostProductionStart(scope);
            }
        }

        private async Task GetSubsAndPostProductionStart(IServiceScope scope)
        {
            var webhookContext = scope.ServiceProvider.GetRequiredService<WebhookContext>();
            var subscribers = webhookContext.Webhooks.Where(x => x.IsActive == true && x.SubscribedPlantEvent == "startProduction");
            var messageContext = scope.ServiceProvider.GetRequiredService<MessageContext>();
            var message = await GetStartProductionMessage(messageContext);
            foreach (var s in subscribers)
            {
                await _messageSender.PostAsync(message, s);
            }
        }

        private async Task<StartProductionMessage> GetStartProductionMessage(MessageContext context)
        {
            var message = new StartProductionMessage();
            context.StartProductionMessages.Add(message);
            await context.SaveChangesAsync();
            return message;
        }
    }
}
