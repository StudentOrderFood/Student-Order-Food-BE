namespace OrderFood_BE.Application.Models.Requests.Feedback
{
    public class CreateFeedbackRequest
    {
        public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; }
        public int Rating { get; set; }
        public string Content { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
    }
}
