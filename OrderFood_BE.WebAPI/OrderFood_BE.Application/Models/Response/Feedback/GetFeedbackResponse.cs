namespace OrderFood_BE.Application.Models.Response.Feedback
{
    public class GetFeedbackResponse
    {
        public Guid Id { get; set; }
        public int Rating { get; set; }
        public string Content { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
