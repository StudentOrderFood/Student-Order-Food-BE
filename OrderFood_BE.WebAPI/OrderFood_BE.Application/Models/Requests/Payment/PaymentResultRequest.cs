using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderFood_BE.Application.Models.Requests.Payment
{
    public class PaymentResultRequest
    {
        public long OrderCode { get; set; }
        public string Status { get; set; } // Ex: PAID, CANCELED, FAILED
    }
}
