using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using OrderFood_BE.Application.Models.Requests.Order;
using OrderFood_BE.Application.Services;

namespace OrderFood_BE.Infrastructure.Services
{
    public class TemporaryOrderCacheService : ITemporaryOrderCacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<TemporaryOrderCacheService> _logger;

        public TemporaryOrderCacheService(IMemoryCache memoryCache, ILogger<TemporaryOrderCacheService> logger)
        {
            _memoryCache = memoryCache;
            _logger = logger;
        }

        public void SaveOrder(BankingOrderRequest order)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5)); // tự xoá nếu không truy cập trong 5 phút

            _memoryCache.Set(order.PayosOrderCode, order, cacheEntryOptions);

            _logger.LogInformation($"[CACHE] Order {order.PayosOrderCode} cached at {DateTime.UtcNow}");
        }

        public bool TryGetOrder(long orderCode, out BankingOrderRequest? order)
        {
            return _memoryCache.TryGetValue(orderCode, out order);
        }

        public void RemoveOrder(long orderCode)
        {
            _memoryCache.Remove(orderCode);
            _logger.LogInformation($"[CACHE] Order {orderCode} removed at {DateTime.UtcNow}");
        }
    }
}
