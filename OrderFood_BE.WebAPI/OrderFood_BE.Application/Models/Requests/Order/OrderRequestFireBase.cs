namespace OrderFood_BE.Application.Models.Requests.Order
{
    public class OrderRequestFireBase
    {
        public Guid CustomerId { get; set; }
        public Guid ShopId { get; set; }
        public List<OrderItemFireBase> OrderItems { get; set; }
        public string PaymentMethod { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class OrderItemFireBase
    {
        public Guid ItemId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
