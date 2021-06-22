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
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;
        private readonly IServiceScopeFactory _scopeFactory;

        /// <summary>
        /// Service handels material production start events
        /// </summary>
        public StartProductionService(ILogger<StartProductionService> logger,
                                      ITimerTillNextProductionStart timer,
                                      HttpClient httpClient,
                                      IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _timer = timer;
            _httpClient = httpClient;
            _scopeFactory = scopeFactory;

            SubscribeToAndStartTimer();
        }

        private void SubscribeToAndStartTimer()
        {
            _timer.TimeElapsed += NewProductionStartHandler;
            _timer.Start();
        }

        private async void NewProductionStartHandler(object sender, System.Timers.ElapsedEventArgs e)
        {
            const string mediaType = "application/json";
            using (var scope = _scopeFactory.CreateScope())
            {
                var webhookContext = scope.ServiceProvider.GetRequiredService<WebhookContext>();
                var subscribers = webhookContext.Webhooks.Where(x => x.IsActive == true && x.SubscribedPlantEvent == "startProduction");
                var messageContext = scope.ServiceProvider.GetRequiredService<MessageContext>();
                var spMessageInJson = await GetStartProductionMessageInJson(messageContext);
                foreach (var s in subscribers)
                {
                    await _httpClient.PostAsync(s.CallbackUrl, new StringContent(spMessageInJson, Encoding.UTF8, mediaType));
                    _logger.LogInformation($"Start production event was sent to {s.CallbackUrl}");
                }
            }
        }

        private async Task<string> GetStartProductionMessageInJson(MessageContext context)
        {
            var message = new StartProductionMessage();
            context.StartProductionMessages.Add(message);
            await context.SaveChangesAsync();

            return JsonSerializer.Serialize(message);
        }
    }
}
