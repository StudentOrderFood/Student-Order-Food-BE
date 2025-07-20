using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderFood_BE.Domain.Base;

namespace OrderFood_BE.Domain.Entities
{
    public class User : BaseEntity<Guid>
    {
        public string FullName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Avatar { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime Dob { get; set; }
        public bool IsActive { get; set; } = true;
        public decimal WalletBalance { get; set; } = 0;

        // Navigation properties
        public Guid RoleId { get; set; }
        public Role Role { get; set; }

        public ICollection<Shop> Shops { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<Feedback> Feedbacks { get; set; }
        public ICollection<HistoryTransaction> HistoryTransactions { get; set; }
    }
}
