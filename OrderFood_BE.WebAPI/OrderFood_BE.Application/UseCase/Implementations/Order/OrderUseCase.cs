using System.Text.Json;
using OrderFood_BE.Application.Models.Requests.Order;
using OrderFood_BE.Application.Models.Response.Order;
using OrderFood_BE.Application.Models.Response.Payment;
using OrderFood_BE.Application.Repositories;
using OrderFood_BE.Application.UseCase.Interfaces.Order;
using OrderFood_BE.Domain.Entities;
using OrderFood_BE.Shared.Common;
using OrderFood_BE.Shared.Enums;

namespace OrderFood_BE.Application.UseCase.Implementations.Order
{
    public class OrderUseCase : IOrderUseCase
    {
        private readonly IOrderRepository _orderRepository;

        public OrderUseCase(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<ApiResponse<GetOrderResponse>> CreateOrderAsync(CreateOrderRequest request)
        {
            if (request == null || request.OrderItems == null || !request.OrderItems.Any())
                return ApiResponse<GetOrderResponse>.Fail("Bad request! Order items cannot be empty");

            var newOrder = new Domain.Entities.Order
            {
                CustomerId = request.CustomerId,
                ShopId = request.ShopId,
                VoucherId = request.VoucherId,
                TotalAmount = request.OrderItems.Sum(oi => oi.Price * oi.Quantity),
                Status = OrderStatusEnum.Pending.ToString(),
                OrderTime = DateTime.Now,
                OrderItems = request.OrderItems.Select(oi => new OrderItem
                {
                    ItemId = oi.ItemId,
                    Quantity = oi.Quantity,
                    Price = oi.Price,
                    Note = oi.Note
                }).ToList()
            };

            await _orderRepository.AddAsync(newOrder);
            await _orderRepository.SaveChangesAsync();

            var response = new GetOrderResponse
            {
                Id = newOrder.Id,
                CustomerId = newOrder.CustomerId,
                ShopId = newOrder.ShopId,
                VoucherId = newOrder.VoucherId,
                TotalAmount = newOrder.TotalAmount,
                Status = newOrder.Status,
                OrderTime = newOrder.OrderTime,
                OrderItems = newOrder.OrderItems.Select(oi => new GetOrderItemResponse
                {
                    ItemId = oi.ItemId,
                    ItemName = oi.Item?.Name ?? "",
                    Quantity = oi.Quantity,
                    Price = oi.Price,
                    Note = oi.Note
                }).ToList()
            };

            return ApiResponse<GetOrderResponse>.Ok(response, "Create order successfully");
        }


        public async Task<ApiResponse<GetOrderResponse>> GetOrderByIdAsync(Guid id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
                return ApiResponse<GetOrderResponse>.Fail("Order not found");

            var response = new GetOrderResponse
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                ShopId = order.ShopId,
                VoucherId = order.VoucherId,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                OrderTime = order.OrderTime,
                OrderItems = order.OrderItems.Select(oi => new GetOrderItemResponse
                {
                    ItemId = oi.ItemId,
                    ItemName = oi.Item?.Name ?? "",
                    Quantity = oi.Quantity,
                    Price = oi.Price,
                    Note = oi.Note
                }).ToList()
            };

            return ApiResponse<GetOrderResponse>.Ok(response, "Order retrieved successfully");
        }

        public async Task<ApiResponse<IEnumerable<GetOrderResponse>>> GetOrdersByCustomerIdAsync(Guid customerId)
        {
            var orders = await _orderRepository.GetOrdersByCustomerIdAsync(customerId);
            if (orders == null || !orders.Any())
                return ApiResponse<IEnumerable<GetOrderResponse>>.Fail("No orders found for this customer");

            var response = orders.Select(order => new GetOrderResponse
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                ShopId = order.ShopId,
                VoucherId = order.VoucherId,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                OrderTime = order.OrderTime,
                OrderItems = order.OrderItems.Select(oi => new GetOrderItemResponse
                {
                    ItemId = oi.ItemId,
                    ItemName = oi.Item?.Name ?? "",
                    Quantity = oi.Quantity,
                    Price = oi.Price,
                    Note = oi.Note
                }).ToList()
            });

            return ApiResponse<IEnumerable<GetOrderResponse>>.Ok(response, "Orders retrieved successfully");
        }

        public async Task<ApiResponse<IEnumerable<GetOrderResponse>>> GetOrdersByShopIdAsync(Guid shopId)
        {
            var orders = await _orderRepository.GetOrdersByShopIdAsync(shopId);
            if (orders == null || !orders.Any())
                return ApiResponse<IEnumerable<GetOrderResponse>>.Fail("No orders found for this shop");

            var response = orders.Select(order => new GetOrderResponse
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                ShopId = order.ShopId,
                VoucherId = order.VoucherId,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                OrderTime = order.OrderTime,
                OrderItems = order.OrderItems.Select(oi => new GetOrderItemResponse
                {
                    ItemId = oi.ItemId,
                    ItemName = oi.Item?.Name ?? "",
                    Quantity = oi.Quantity,
                    Price = oi.Price,
                    Note = oi.Note
                }).ToList()
            });

            return ApiResponse<IEnumerable<GetOrderResponse>>.Ok(response, "Orders retrieved successfully");
        }

        public async Task<ApiResponse<string>> CancelOrderAsync(Guid id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
                return ApiResponse<string>.Fail("Order not found");

            if (order.Status != OrderStatusEnum.Pending.ToString())
                return ApiResponse<string>.Fail("Only pending orders can be cancelled");

            order.Status = OrderStatusEnum.Cancelled.ToString();
            await _orderRepository.UpdateAsync(order);
            await _orderRepository.SaveChangesAsync();

            return ApiResponse<string>.Ok("", "Order cancelled successfully");

        }

        public async Task<ApiResponse<string>> CreateOrderFromFirebaseAsync(string firebaseId, OrderRequestFireBase request)
        {
            var requestJson = JsonSerializer.Serialize(request, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            if (request == null)
            {
                return ApiResponse<string>.Fail("Request is null");
            }
            if (string.IsNullOrEmpty(request.CustomerId.ToString()) || string.IsNullOrEmpty(request.ShopId.ToString()))
            {
                return ApiResponse<string>.Fail("CustomerId or ShopId is missing");
            }
            if (request.OrderItems == null || !request.OrderItems.Any())
            {
                return ApiResponse<string>.Fail("Order must contain at least one item");
            }

            Console.WriteLine("FirebaseId: " + firebaseId);
            Console.WriteLine("Request: " + JsonSerializer.Serialize(request));

            var newOrder = new Domain.Entities.Order();

            newOrder.TotalAmount = request.TotalAmount;
            newOrder.Status = OrderStatusEnum.Pending.ToString();
            newOrder.OrderTime = DateTime.Now;
            newOrder.FirebaseId = firebaseId;
            newOrder.CustomerId = request.CustomerId;
            newOrder.ShopId = request.ShopId;
            newOrder.OrderItems = request.OrderItems.Select(oi => new OrderItem
            {
                ItemId = oi.ItemId,
                Quantity = oi.Quantity,
                Price = oi.Price,
            }).ToList();
            newOrder.PaymentMethod = request.PaymentMethod;

            var newItem = await _orderRepository.AddAsync(newOrder);
            await _orderRepository.SaveChangesAsync();
            return ApiResponse<string>.Ok(newItem.Id.ToString(), "Add order into DB successfully");
        }

        public async Task<int> UpdateOrderStatusFromFirebaseAsync(string firebaseId, string newStatus)
        {
            if (string.IsNullOrEmpty(firebaseId) || string.IsNullOrEmpty(newStatus))
            {
                return -1;
            }
            var order = await _orderRepository.GetOrderByFirebaseId(firebaseId);
            if (order == null)
            {
                return -1;
            }
            order.Status = newStatus;
            await _orderRepository.UpdateAsync(order);
            await _orderRepository.SaveChangesAsync();
            return 1;
        }

        public Task<int> HandleSuccessfulPaymentAsync(BankingOrderRequest model)
        {
            throw new NotImplementedException();
        }
    }
}
