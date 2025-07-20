using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderFood_BE.Application.Models.Requests.Order;

namespace OrderFood_BE.Application.Services
{
    public interface IFirebaseOrderSyncService
    {
        Task PushOrderAsync(BankingOrderRequest order);
    }
}
