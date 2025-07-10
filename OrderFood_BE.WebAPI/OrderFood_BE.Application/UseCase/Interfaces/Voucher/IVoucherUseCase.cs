using OrderFood_BE.Application.Models.Requests.Voucher;
using OrderFood_BE.Application.Models.Response.Voucher;
using OrderFood_BE.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderFood_BE.Application.UseCase.Interfaces.Voucher
{
    public interface IVoucherUseCase
    {
        Task<ApiResponse<GetVoucherResponse>> CreateVoucherAsync(CreateVoucherRequest request);
        Task<ApiResponse<GetVoucherResponse>> UpdateVoucherAsync(Guid id, UpdateVoucherRequest request);
        Task<ApiResponse<string>> DeleteVoucherAsync(Guid id);
        Task<ApiResponse<GetVoucherResponse>> GetVoucherByIdAsync(Guid id);
        Task<ApiResponse<GetVoucherResponse>> GetVoucherByCodeAsync(string code);
        Task<ApiResponse<IEnumerable<GetVoucherResponse>>> GetVouchersByShopIdAsync(Guid shopId);
        Task<ApiResponse<IEnumerable<GetVoucherResponse>>> GetAllAvailableVouchersAsync();
    }
}
