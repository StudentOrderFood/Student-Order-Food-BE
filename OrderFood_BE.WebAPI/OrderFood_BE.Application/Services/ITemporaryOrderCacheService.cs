using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderFood_BE.Application.Models.Requests.Order;

namespace OrderFood_BE.Application.Services
{
    public interface ITemporaryOrderCacheService
    {
        void SaveOrder(BankingOrderRequest order);
        bool TryGetOrder(long orderCode, out BankingOrderRequest? order);
        void RemoveOrder(long orderCode);
    }
}
