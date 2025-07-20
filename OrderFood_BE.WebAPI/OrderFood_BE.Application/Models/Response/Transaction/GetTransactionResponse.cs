using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderFood_BE.Application.Models.Response.Transaction
{
    public class GetTransactionResponse
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string OrderCode { get; set; } 
        public string Status { get; set; }
        public DateTime PaymentTime { get; set; }
        public DateTime CreatedAt { get; set; }

        public Guid UserId { get; set; }

        public Guid? OrderId { get; set; }
    }
}
