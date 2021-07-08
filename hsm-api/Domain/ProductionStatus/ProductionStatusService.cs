using hsm_api.ConfigurationOptions.TimerSettings;
using hsm_api.Infrastructure;
using hsm_api.Models;
using hsm_api.Models.Messages;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hsm_api.Domain.ProductionStatus
{
    public class ProductionStatusService
    {
        private readonly IDynamicIntervalTimer<ProductionStatusTimerSettings> _timer;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ProductionStatusHttpMessageSender _messageSender;

        /// <summary>
        /// Service handels production status events
        /// </summary>
        public ProductionStatusService(IDynamicIntervalTimer<ProductionStatusTimerSettings> timer,
                                       IServiceScopeFactory scopeFactory,
                                       ProductionStatusHttpMessageSender messageSender)
        {
            _timer = timer;
            _scopeFactory = scopeFactory;
            _messageSender = messageSender;

            SubscribeToTimer();
        }

        private void SubscribeToTimer()
        {
            _timer.TimeElapsed += ProductionStatusHandler;
        }

        private async void ProductionStatusHandler(object sender, System.Timers.ElapsedEventArgs e)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                await GetSubsAndPostProductionStatus(scope);
            }
        }

        private async Task GetSubsAndPostProductionStatus(IServiceScope scope)
        {
            var webhookContext = scope.ServiceProvider.GetRequiredService<WebhookContext>();
            var subscribers = GetSubscribers(webhookContext);
            var messageContext = scope.ServiceProvider.GetRequiredService<MessageContext>();

            (DateTime StateDate, float MillSpeed, float CoilingSpeed) statusData = (DateTime.Now, 0, 0);
            foreach (var s in subscribers)
            {
                var message = await GetProductionStatusMessage(messageContext, statusData);
                await _messageSender.PostAsync(message, s);
                statusData.MillSpeed = message.MillSpeed;
                statusData.CoilingSpeed = message.CoilingSpeed;
            }
        }

        private IEnumerable<Webhook> GetSubscribers(WebhookContext webhookContext)
        {
            string eventName = MessageService.GetMessageNameFromMessageClass(typeof(ProductionStatusMessage));
            return webhookContext.Webhooks.Where(x => x.IsActive == true && x.SubscribedPlantEvent == eventName);
        }

        private async Task<ProductionStatusMessage> GetProductionStatusMessage(MessageContext context,
                                                                               (DateTime StateDate, float MillSpeed, float CoilingSpeed) statusData)
        {
            var message = new ProductionStatusMessage();
            message.StateDate = statusData.StateDate;
            message.MillSpeed = statusData.MillSpeed;
            message.CoilingSpeed = statusData.CoilingSpeed;
            context.ProductionStatusMessages.Add(message);
            await context.SaveChangesAsync();
            return message;
        }
    }
}
