using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderFood_BE.Application.Models.Requests.Order
{
    public class BankingOrderRequest
    {
        public Guid CustomerId { get; set; }
        public Guid ShopId { get; set; }
        public List<OrderItemModel> OrderItems { get; set; }
        public string OrderStatus { get; set; }
        public string PaymentMethod { get; set; }
        public decimal TotalAmount { get; set; }
        public long PayosOrderCode { get; set; }
    }

    public class OrderItemModel
    {
        public Guid ItemId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
