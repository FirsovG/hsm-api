using hsm_api.Models;
using hsm_api.Models.Messages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace hsm_api.Domain.ProductionStatus
{
    public class ProductionStatusHttpMessageSender
    {
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;

        public ProductionStatusHttpMessageSender(HttpClient httpClient,
                                                 ILogger<ProductionStatusHttpMessageSender> logger)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task PostAsync(ProductionStatusMessage message, Webhook subscriber)
        {
            const string mediaType = "application/json";
            var messageAsJson = JsonSerializer.Serialize(message);
            await _httpClient.PostAsync(subscriber.CallbackUrl, new StringContent(messageAsJson, Encoding.UTF8, mediaType));
            _logger.LogInformation($"Production status event was sent to {subscriber.CallbackUrl}");
        }
    }
}
