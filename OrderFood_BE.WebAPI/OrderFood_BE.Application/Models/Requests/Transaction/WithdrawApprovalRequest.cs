using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderFood_BE.Application.Models.Requests.Transaction
{
    public class WithdrawApprovalRequest
    {
        public Guid TransactionId { get; set; }
        public bool IsApproved { get; set; }
        public string? AdminNote { get; set; }
    }
}
