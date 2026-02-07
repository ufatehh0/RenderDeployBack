using CodeDungeon.Data;
using CodeDungeon.DTOs;
using CodeDungeon.DTOs.UserDTOs;
using CodeDungeon.Models.Entities;
using CodeDungeon.Services.Abstract;
using Microsoft.EntityFrameworkCore;

namespace CodeDungeon.Services.Concrete
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserGetDto>> GetAllUsersAsync()
        {
            return await _context.Users
                .Select(u => MapToGetDto(u))
                .ToListAsync();
        }

        public async Task<UserGetDto?> GetUserByIdAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            return user == null ? null : MapToGetDto(user);
        }

        public async Task<UserGetDto?> GetUserByUserNameAsync(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            return user == null ? null : MapToGetDto(user);
        }

        public async Task<bool> CreateUserAsync(UserCreateDto dto)
        {
            if (await _context.Users.AnyAsync(x => x.Email == dto.Email || x.Username == dto.Username))
                return false;

            var user = new User
            {
                Username = dto.Username,
                Name = dto.Name,
                Surname = dto.Surname,
                FatherName = dto.FatherName,
                FinCode = dto.FinCode,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                BirthDate = dto.BirthDate.ToUniversalTime(),
                Role = dto.Role,
                IsPasswordConfirmed = false,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateUserAsync(Guid id, UserEditDTO dto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            user.Username = dto.Username;
            user.Name = dto.Name;
            user.Surname = dto.Surname;
            user.FatherName = dto.FatherName;
            user.Email = dto.Email;
            user.PhoneNumber = dto.PhoneNumber;
            user.BirthDate = dto.BirthDate.ToUniversalTime();
            user.Role = dto.Role;
            user.FinCode = dto.FinCode;

            _context.Users.Update(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            return await _context.SaveChangesAsync() > 0;
        }

        private static UserGetDto MapToGetDto(User u) => new UserGetDto
        {
            Id = u.Id,
            Role = u.Role,
            Username = u.Username,
            Name = u.Name,
            Surname = u.Surname,
            FatherName = u.FatherName,
            Email = u.Email,
            FinCode = u.FinCode,
            PhoneNumber = u.PhoneNumber,
            BirthDate = u.BirthDate,
            IsPasswordConfirmed = u.IsPasswordConfirmed,
            IsActive = u.IsActive,
            CreatedAt = u.CreatedAt
        };
    }
}