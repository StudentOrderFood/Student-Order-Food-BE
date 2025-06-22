using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderFood_BE.Domain.Entities;

namespace OrderFood_BE.Application.Repositories
{
    public interface IUserRepository : IGenericRepository<User, Guid>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<bool> ExistsByEmailAsync(string email);
        Task<bool> ExistsAsync(string value);
        Task<User?> GetUserByEmailPhoneOrUserName(string val);
        Task<User?> GetUserByIdAsync(Guid userId);
        Task<IEnumerable<User>?> GetAllUserAsync();
    }
}
