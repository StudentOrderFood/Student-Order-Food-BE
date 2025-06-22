using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderFood_BE.Application.Models.Requests.Shop;

namespace OrderFood_BE.Application.UseCase.Interfaces.Shop
{
    public interface IShopUseCase
    {
        /// <summary>
        /// Creates a new shop request for the specified owner.
        /// </summary>
        /// <param name="request">The request containing shop details and owner information.</param>
        /// <returns>
        /// A message indicating the result of the operation:
        /// - "User is not authorized to create a shop." if the user is not a shop owner.
        /// - "The request has been submitted and is awaiting approval." if the shop creation request is successful.
        /// </returns>
        Task<string> CreateShopAsync(CreateShopRequest request);
    }
}
