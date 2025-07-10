namespace OrderFood_BE.Application.Models.Requests.Order
{
    public class CreateOrderRequest
    {
        public Guid CustomerId { get; set; }
        public Guid ShopId { get; set; }
        public Guid? VoucherId { get; set; }
        public List<OrderItemRequest> OrderItems { get; set; }
    }

    public class OrderItemRequest
    {
        public Guid ItemId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Note { get; set; } = string.Empty;
    }
}
