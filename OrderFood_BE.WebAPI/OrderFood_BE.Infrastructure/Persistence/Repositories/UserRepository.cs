using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrderFood_BE.Application.Repositories;
using OrderFood_BE.Domain.Entities;
using OrderFood_BE.Infrastructure.Persistence.DBContext;
using OrderFood_BE.Shared.Enums;

namespace OrderFood_BE.Infrastructure.Persistence.Repositories
{
    public class UserRepository(AppDbContext context) : GenericRepository<User, Guid>(context), IUserRepository
    {
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == email && u.IsDeleted == false);
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email && u.IsDeleted == false) != null;
        }

        public Task<bool> ExistsAsync(string value)
        {
            return _context.Users
                .AsNoTracking()
                .AnyAsync(u => (u.Email == value || u.UserName == value || u.Phone == value) && u.IsDeleted == false);
        }

        public Task<User?> GetUserByEmailPhoneOrUserName(string val)
        {
            return _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => (u.Email == val || u.UserName == val || u.Phone == val) && u.IsDeleted == false);
        }

        public async Task<User?> GetUserByIdAsync(Guid userId)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == userId && u.IsDeleted == false && u.IsActive == true);
        }

        public async Task<IEnumerable<User>?> GetAllUserAsync()
        {
            return await _context.Users
                .Include(u => u.Role)
                .Where(u => u.IsDeleted == false && u.IsActive == true && u.Role.Name != RoleEnum.Admin.ToString())
                .ToListAsync();
        }
    }
}
