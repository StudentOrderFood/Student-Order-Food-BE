using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using OrderFood_BE.Application.Models.Requests.Order;

namespace OrderFood_BE.Application.Services
{
    public class FirebaseOrderSyncService : IFirebaseOrderSyncService
    {
        private readonly HttpClient _httpClient;
        private const string FirebaseBaseUrl = "https://student-order-food-default-rtdb.asia-southeast1.firebasedatabase.app/";
        private const string OrdersNode = "orders";

        public FirebaseOrderSyncService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task PushOrderAsync(BankingOrderRequest order)
        {
            //Mapping
            var firebaseOrder = new OrderRequestFireBase
            {
                CustomerId = order.CustomerId,
                ShopId = order.ShopId,
                PaymentMethod = order.PaymentMethod,
                TotalAmount = order.TotalAmount,
                OrderItems = order.OrderItems.Select(item => new OrderItemFireBase
                {
                    ItemId = item.ItemId,
                    Quantity = item.Quantity,
                    Price = item.Price
                }).ToList()
            };

            var url = $"{FirebaseBaseUrl}{OrdersNode}.json";
            var json = JsonSerializer.Serialize(firebaseOrder);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Firebase push failed: {response.StatusCode}");
            }
        }
    }
}
