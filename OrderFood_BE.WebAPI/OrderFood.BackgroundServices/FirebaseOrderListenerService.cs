using System.Text.Json;
using Google.Apis.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrderFood_BE.Application.Models.Requests.Order;
using OrderFood_BE.Application.UseCase.Interfaces.Order;
using IHttpClientFactory = System.Net.Http.IHttpClientFactory;

namespace OrderFood.BackgroundServices
{
    public class FirebaseOrderListenerService : BackgroundService
    {
        private readonly ILogger<FirebaseOrderListenerService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private const string FirebaseDbUrl = "https://student-order-food-default-rtdb.asia-southeast1.firebasedatabase.app/orders.json";

        public FirebaseOrderListenerService(
            ILogger<FirebaseOrderListenerService> logger,
            IHttpClientFactory httpClientFactory,
                    IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Listening Firebase for orders...");

            var client = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, FirebaseDbUrl);
            request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("text/event-stream"));

            var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, stoppingToken);
            var stream = await response.Content.ReadAsStreamAsync(stoppingToken);
            using var reader = new StreamReader(stream);

            string? eventType = null;

            while (!reader.EndOfStream && !stoppingToken.IsCancellationRequested)
            {
                var line = await reader.ReadLineAsync(stoppingToken);
                if (string.IsNullOrWhiteSpace(line)) continue;

                if (line.StartsWith("event: "))
                {
                    eventType = line.Substring("event: ".Length);
                    continue;
                }

                if (line.StartsWith("data: "))
                {
                    var json = line.Substring("data: ".Length);
                    if (json == "null" || eventType == "keep-alive") continue;

                    try
                    {
                        using var doc = JsonDocument.Parse(json);
                        var root = doc.RootElement;

                        if (!root.TryGetProperty("path", out var pathElement) ||
                            !root.TryGetProperty("data", out var dataElement))
                        {
                            _logger.LogWarning("Firebase event missing required fields");
                            continue;
                        }

                        var path = pathElement.GetString()?.Trim('/');
                        using var scope = _serviceScopeFactory.CreateScope();
                        var orderUseCase = scope.ServiceProvider.GetRequiredService<IOrderUseCase>();

                        if (dataElement.ValueKind == JsonValueKind.Object)
                        {
                            var options = new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true
                            };

                            if (string.IsNullOrEmpty(path)) // Khi mới chạy app: toàn bộ data
                            {
                                foreach (var property in dataElement.EnumerateObject())
                                {
                                    var firebaseId = property.Name;
                                    var orderJson = property.Value.GetRawText();
                                    var order = JsonSerializer.Deserialize<OrderRequestFireBase>(orderJson, options);

                                    if (order != null)
                                    {
                                        _logger.LogInformation($"[Firebase] Initial load - Order ID: {firebaseId}, Payment: {order.TotalAmount}");
                                    }
                                }
                             }
                            else // Khi push thêm 1 đơn mới
                            {
                                var firebaseId = path.Split('/')[0]; // Lấy order id
                                var orderJson = dataElement.GetRawText();
                                var order = JsonSerializer.Deserialize<OrderRequestFireBase>(orderJson, options);

                                if (order != null)
                                {
                                    // Call UseCase chỗ này là xong
                                    _logger.LogInformation($"[Firebase] New/Updated order: {firebaseId}, Payment: {order.TotalAmount}");
                                }
                            }
                        }
                        else
                        {
                            _logger.LogInformation($"[Firebase] Skipped path: {path}, not an object.");
                        }

                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error while processing Firebase stream");
                    }
                }
            }

            _logger.LogInformation("Firebase order listener stopped.");
        }
    }
}
