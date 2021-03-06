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
        /// Service handels material production finish events
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
            _timer.TimeElapsed += ProductionFinishHandler;
        }

        private async void ProductionFinishHandler(object sender, System.Timers.ElapsedEventArgs e)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                await GetSubsAndPostProductionFinish(scope);
            }
        }

        private async Task GetSubsAndPostProductionFinish(IServiceScope scope)
        {
            var webhookContext = scope.ServiceProvider.GetRequiredService<WebhookContext>();
            var subscribers = GetSubscribers(webhookContext);
            var messageContext = scope.ServiceProvider.GetRequiredService<MessageContext>();
            (string CoilId, DateTime ProductionFinishDate, float Width, float Thickness, float Weight) finishedCoilData = 
                (GetIdOfOldestCoilAtProductionStart(messageContext), DateTime.Now, 0, 0, 0);
            SetCoilProductionStartToProduced(messageContext, finishedCoilData.CoilId);
            foreach (var s in subscribers)
            {
                var message = await GetFinishProductionMessage(messageContext, finishedCoilData);
                await _messageSender.PostAsync(message, s);
                finishedCoilData.CoilId = message.CoilId;
                finishedCoilData.Width = message.Width;
                finishedCoilData.Thickness = message.Thickness;
                finishedCoilData.Weight = message.Weight;
            }
        }

        private string GetIdOfOldestCoilAtProductionStart(MessageContext messageContext) =>
            messageContext.StartProductionMessages.Where(x => !x.WasProduced)
                                                  .OrderBy(x => x.ProductionStartDate)
                                                  .FirstOrDefault()
                                                  ?.CoilId;

        private void SetCoilProductionStartToProduced(MessageContext messageContext, string coilId)
        {
            var productionStart = messageContext.StartProductionMessages.FirstOrDefault(x => x.CoilId == coilId);
            if (productionStart != null)
                productionStart.WasProduced = true;
        }

        private IEnumerable<Webhook> GetSubscribers(WebhookContext webhookContext)
        {
            string eventName = MessageService.GetMessageNameFromMessageClass(typeof(FinishProductionMessage));
            return webhookContext.Webhooks.Where(x => x.IsActive == true && x.SubscribedPlantEvent == eventName);
        }

        private async Task<FinishProductionMessage> GetFinishProductionMessage(MessageContext context, (string CoilId, DateTime ProductionFinishDate, 
                                                                                                        float Width, float Thickness, float Weight) finishedCoilData)
        {
            var message = new FinishProductionMessage();
            message.CoilId = finishedCoilData.CoilId;
            message.ProductionFinishDate = finishedCoilData.ProductionFinishDate;
            message.Width = finishedCoilData.Width;
            message.Thickness = finishedCoilData.Thickness;
            message.Weight = finishedCoilData.Weight;
            context.FinishProductionMessages.Add(message);
            await context.SaveChangesAsync();
            return message;
        }
    }
}
