using OrderFood_BE.Application.Models.Requests.Voucher;
using OrderFood_BE.Application.Models.Response.Voucher;
using OrderFood_BE.Application.Repositories;
using OrderFood_BE.Application.UseCase.Interfaces.Voucher;
using OrderFood_BE.Shared.Common;

namespace OrderFood_BE.Application.UseCase.Implementations.Voucher
{
    public class VoucherUseCase : IVoucherUseCase
    {
        private readonly IVoucherRepository _voucherRepository;

        public VoucherUseCase(IVoucherRepository voucherRepository)
        {
            _voucherRepository = voucherRepository;
        }

        public async Task<ApiResponse<GetVoucherResponse>> CreateVoucherAsync(CreateVoucherRequest request)
        {
            if (request == null)
                return ApiResponse<GetVoucherResponse>.Fail("Bad request!");

            var newVoucher = new Domain.Entities.Voucher
            {
                VoucherCode = request.VoucherCode,
                Title = request.Title,
                Description = request.Description,
                DiscountValue = request.DiscountValue,
                Count = request.Count,
                IsActive = true,
                ShopId = request.ShopId,
                CreatedAt = DateTime.Now
            };

            await _voucherRepository.AddAsync(newVoucher);
            await _voucherRepository.SaveChangesAsync();

            var response = new GetVoucherResponse
            {
                Id = newVoucher.Id,
                VoucherCode = newVoucher.VoucherCode,
                Title = newVoucher.Title,
                Description = newVoucher.Description,
                DiscountValue = newVoucher.DiscountValue,
                Count = newVoucher.Count,
                IsActive = newVoucher.IsActive,
                ShopId = newVoucher.ShopId
            };

            return ApiResponse<GetVoucherResponse>.Ok(response, "Create voucher successfully");
            
        }

        public async Task<ApiResponse<GetVoucherResponse>> UpdateVoucherAsync(Guid id, UpdateVoucherRequest request)
        {
            if (request == null)
                return ApiResponse<GetVoucherResponse>.Fail("Bad request!");

            var voucher = await _voucherRepository.GetByIdAsync(id);
            if (voucher == null)
                return ApiResponse<GetVoucherResponse>.Fail("Voucher not found");

            voucher.VoucherCode = request.VoucherCode;
            voucher.Title = request.Title;
            voucher.Description = request.Description;
            voucher.DiscountValue = request.DiscountValue;
            voucher.Count = request.Count;
            voucher.IsActive = request.IsActive;

            _voucherRepository.Update(voucher);
            await _voucherRepository.SaveChangesAsync();

            var response = new GetVoucherResponse
            {
                Id = voucher.Id,
                VoucherCode = voucher.VoucherCode,
                Title = voucher.Title,
                Description = voucher.Description,
                DiscountValue = voucher.DiscountValue,
                Count = voucher.Count,
                IsActive = voucher.IsActive,
                ShopId = voucher.ShopId
            };

            return ApiResponse<GetVoucherResponse>.Ok(response, "Voucher updated successfully");
        }

        public async Task<ApiResponse<string>> DeleteVoucherAsync(Guid id)
        {
            var voucher = await _voucherRepository.GetByIdAsync(id);
            if (voucher == null)
                return ApiResponse<string>.Fail("Voucher not found");

            await _voucherRepository.SoftDeleteAsync(voucher);
            await _voucherRepository.SaveChangesAsync();
            return ApiResponse<string>.Ok("", "Voucher deleted successfully");
        }

        public async Task<ApiResponse<GetVoucherResponse>> GetVoucherByIdAsync(Guid id)
        {
            var voucher = await _voucherRepository.GetByIdAsync(id);
            if (voucher == null)
                return ApiResponse<GetVoucherResponse>.Fail("Voucher not found");

            var response = new GetVoucherResponse
            {
                Id = voucher.Id,
                VoucherCode = voucher.VoucherCode,
                Title = voucher.Title,
                Description = voucher.Description,
                DiscountValue = voucher.DiscountValue,
                Count = voucher.Count,
                IsActive = voucher.IsActive,
                ShopId = voucher.ShopId
            };

            return ApiResponse<GetVoucherResponse>.Ok(response, "Voucher retrieved successfully");
        }

        public async Task<ApiResponse<GetVoucherResponse>> GetVoucherByCodeAsync(string code)
        {
            var voucher = await _voucherRepository.GetByVoucherCodeAsync(code);
            if (voucher == null)
                return ApiResponse<GetVoucherResponse>.Fail("Voucher not found");

            var response = new GetVoucherResponse
            {
                Id = voucher.Id,
                VoucherCode = voucher.VoucherCode,
                Title = voucher.Title,
                Description = voucher.Description,
                DiscountValue = voucher.DiscountValue,
                Count = voucher.Count,
                IsActive = voucher.IsActive,
                ShopId = voucher.ShopId
            };

            return ApiResponse<GetVoucherResponse>.Ok(response, "Voucher retrieved successfully");
        }

        public async Task<ApiResponse<IEnumerable<GetVoucherResponse>>> GetVouchersByShopIdAsync(Guid shopId)
        {
            var vouchers = await _voucherRepository.GetVouchersByShopIdAsync(shopId);
            if (vouchers == null || !vouchers.Any())
                return ApiResponse<IEnumerable<GetVoucherResponse>>.Fail("No vouchers found for this shop");

            var responseList = vouchers.Select(v => new GetVoucherResponse
            {
                Id = v.Id,
                VoucherCode = v.VoucherCode,
                Title = v.Title,
                Description = v.Description,
                DiscountValue = v.DiscountValue,
                Count = v.Count,
                IsActive = v.IsActive,
                ShopId = v.ShopId
            });

            return ApiResponse<IEnumerable<GetVoucherResponse>>.Ok(responseList, "Vouchers retrieved successfully");
        }

        public async Task<ApiResponse<IEnumerable<GetVoucherResponse>>> GetAvailableVouchersAsync()
        {
            var vouchers = await _voucherRepository.GetAvailableVouchersAsync();
            if (vouchers == null || !vouchers.Any())
                return ApiResponse<IEnumerable<GetVoucherResponse>>.Fail("No available vouchers found");

            var responseList = vouchers.Select(v => new GetVoucherResponse
            {
                Id = v.Id,
                VoucherCode = v.VoucherCode,
                Title = v.Title,
                Description = v.Description,
                DiscountValue = v.DiscountValue,
                Count = v.Count,
                IsActive = v.IsActive,
                ShopId = v.ShopId
            });

            return ApiResponse<IEnumerable<GetVoucherResponse>>.Ok(responseList, "Available vouchers retrieved successfully");
        }
    }
}
