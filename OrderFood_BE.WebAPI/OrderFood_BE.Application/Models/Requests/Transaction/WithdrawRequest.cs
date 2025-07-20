using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderFood_BE.Application.Models.Requests.Transaction
{
    public class WithdrawRequest
    {
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
    }
}
