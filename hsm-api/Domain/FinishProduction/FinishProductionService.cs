using hsm_api.ConfigurationOptions.TimerSettings;
using hsm_api.Infrastructure;
using hsm_api.Models;
using hsm_api.Models.Messages;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hsm_api.Domain.FinishProduction
{
    public class FinishProductionService
    {
        private readonly IDynamicIntervalTimer<FinishProductionTimerSettings> _timer;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly FinishProductionHttpMessageSender _messageSender;

        /// <summary>
        /// Service handels material production start events
        /// </summary>
        public FinishProductionService(IDynamicIntervalTimer<FinishProductionTimerSettings> timer,
                                       IServiceScopeFactory scopeFactory,
                                       FinishProductionHttpMessageSender messageSender)
        {
            _timer = timer;
            _scopeFactory = scopeFactory;
            _messageSender = messageSender;

            SubscribeToTimer();
        }

        private void SubscribeToTimer()
        {
            _timer.TimeElapsed += NewProductionStartHandler;
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
            var subscribers = GetSubscribers(webhookContext);
            var messageContext = scope.ServiceProvider.GetRequiredService<MessageContext>();
            var message = await GetFinishProductionMessage(messageContext);
            foreach (var s in subscribers)
            {
                await _messageSender.PostAsync(message, s);
            }
        }

        private IEnumerable<Webhook> GetSubscribers(WebhookContext webhookContext)
        {
            string eventName = MessageService.GetMessageNameFromMessageClass(typeof(FinishProductionMessage));
            return webhookContext.Webhooks.Where(x => x.IsActive == true && x.SubscribedPlantEvent == eventName);
        }

        private async Task<FinishProductionMessage> GetFinishProductionMessage(MessageContext context)
        {
            var message = new FinishProductionMessage();
            context.FinishProductionMessages.Add(message);
            await context.SaveChangesAsync();
            return message;
        }
    }
}
