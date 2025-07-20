using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderFood_BE.Application.Models.Response.Payment
{
    public class PayOSWebhookModel
    {
        public long OrderCode { get; set; }
        public string Status { get; set; }
        public long Amount { get; set; }
        public string Signature { get; set; } // PayOS sẽ gửi chữ ký để xác thực
    }
}
