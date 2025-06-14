using OrderFood_BE.Domain.Base;

namespace OrderFood_BE.Domain.Entities
{
    public class RefreshToken : BaseEntity<Guid>
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiryDate { get; set; }
        public Guid UserId { get; set; }
        public User? User { get; set; }
    }
}
