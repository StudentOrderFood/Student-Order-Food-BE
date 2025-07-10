namespace OrderFood_BE.Application.Models.Response.Order
{
    public class GetOrderResponse
    {
        public Guid Id { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public DateTime OrderTime { get; set; }
        public Guid CustomerId { get; set; }
        public Guid ShopId { get; set; }
        public Guid? VoucherId { get; set; }
        public List<GetOrderItemResponse> OrderItems { get; set; }
    }

    public class GetOrderItemResponse
    {
        public Guid ItemId { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Note { get; set; }
    }

}
