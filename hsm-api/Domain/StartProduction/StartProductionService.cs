using hsm_api.Infrastructure;
using hsm_api.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
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
            using (var scope = _scopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<WebhookContext>();
                var subscribers = db.Webhooks.Where(x => x.IsActive == true && x.SubscribedPlantEvent == "startProduction");
                foreach (var s in subscribers)
                {
                    var message = "{ \"status\": \"productionStarted\", \"webhookId\": \"" + s.Id + "\"}";
                    await _httpClient.PostAsync(s.CallbackUrl, new StringContent(message, Encoding.UTF8, "application/json"));
                    _logger.LogInformation($"Start production event was sent to {s.CallbackUrl}");
                }
            }
        }
    }
}
