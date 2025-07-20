using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderFood_BE.Application.Models.Requests.Payment;
using OrderFood_BE.Application.Models.Response.Payment;

namespace OrderFood_BE.Application.Services
{
    public interface IPayOSService
    {
        Task<string> CreatePaymentLinkAsync(PaymentRequestModel model);
        bool ValidateSignature(PayOSWebhookModel model);
    }
}
