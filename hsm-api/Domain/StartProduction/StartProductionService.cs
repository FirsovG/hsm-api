using hsm_api.ConfigurationOptions.TimerSettings;
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
        private readonly IDynamicIntervalTimer<StartProductionTimerSettings> _timer;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly StartProductionHttpMessageSender _messageSender;

        /// <summary>
        /// Service handels material production start events
        /// </summary>
        public StartProductionService(IDynamicIntervalTimer<StartProductionTimerSettings> timer,
                                      IServiceScopeFactory scopeFactory,
                                      StartProductionHttpMessageSender messageSender)
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

            // coil is generated at first loop run, kept same for others
            (string CoilId, DateTime ProductionStart, bool WasProduced) startedCoilData = 
                (null, DateTime.Now, false);
            foreach (var s in subscribers)
            {
                var message = await GetStartProductionMessage(messageContext, startedCoilData);
                await _messageSender.PostAsync(message, s);
                startedCoilData.CoilId = message.CoilId;
            }
        }

        private IEnumerable<Webhook> GetSubscribers(WebhookContext webhookContext)
        {
            string eventName = MessageService.GetMessageNameFromMessageClass(typeof(StartProductionMessage));
            return webhookContext.Webhooks.Where(x => x.IsActive == true && x.SubscribedPlantEvent == eventName);
        }

        private async Task<StartProductionMessage> GetStartProductionMessage(MessageContext context, 
                                                                             (string CoilId, DateTime ProductionStart, bool WasProduced) coilData)
        {
            var message = new StartProductionMessage();
            message.CoilId = coilData.CoilId;
            message.ProductionStartDate = coilData.ProductionStart;
            message.WasProduced = coilData.WasProduced;
            context.StartProductionMessages.Add(message);
            await context.SaveChangesAsync();
            return message;
        }
    }
}
