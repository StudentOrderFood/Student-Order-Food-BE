using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderFood_BE.Application.Models.Requests.User;
using OrderFood_BE.Application.Models.Response.User;
using OrderFood_BE.Shared.Common;

namespace OrderFood_BE.Application.UseCase.Interfaces.User
{
    public interface IUserUseCase
    {
        /// <summary>
        /// Retrieves a user by their unique identifier.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>
        /// A <see cref="GetUserResponse"/> representing the user if found and active; otherwise, <c>null</c>.
        /// </returns>
        Task<ApiResponse<GetUserResponse>> GetByIdAsync(Guid userId);
        /// <summary>
        /// Retrieves all active and non-deleted users from the repository.
        /// </summary>
        /// <returns>
        /// A list of <see cref="GetUserResponse"/> representing the users, or <c>null</c> if no users are found.
        /// </returns>
        Task<ApiResponse<List<GetUserResponse>>> GetAllAsync();
        /// <summary>
        /// Retrieves all users with the role of ShopOwner from the repository.
        /// </summary>
        /// <remarks>
        /// This method fetches all users and filters them by the ShopOwner role.
        /// Returns a list of <see cref="GetUserResponse"/> for each ShopOwner user,
        /// or <c>null</c> if no such users are found.
        /// </remarks>
        /// <returns>
        /// A list of <see cref="GetUserResponse"/> representing ShopOwner users, or <c>null</c> if none exist.
        /// </returns>
        Task<ApiResponse<List<GetUserResponse>>> GetAllShopOwnerAsync();
        /// <summary>
        /// Retrieves all users with the role of Student from the repository.
        /// </summary>
        /// <remarks>
        /// This method fetches all users and filters them by the Student role.
        /// Returns a list of <see cref="GetUserResponse"/> for each Student user,
        /// or <c>null</c> if no such users are found.
        /// </remarks>
        /// <returns>
        /// A list of <see cref="GetUserResponse"/> representing Student users, or <c>null</c> if none exist.
        /// </returns>
        Task<ApiResponse<List<GetUserResponse>>> GetAllStudentAsync();

        Task<ApiResponse<string>> UpdateProfileAsync(ProfileUpdateRequest request);
        Task<ApiResponse<string>> CheckPhoneNumberExists(string phoneNumber);
        Task<bool> UpdateUserWallet(Guid shopId, decimal amount);
    }
}
