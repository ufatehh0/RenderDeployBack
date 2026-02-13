using CodeDungeon.Data;
using CodeDungeon.DTOs;
using CodeDungeon.DTOs.UserDTOs;
using CodeDungeon.Models.Entities.User;
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

        public async Task<UserGetDto?> GetCurrentUserAsync()
        {
           
            var user = await _context.Users.FirstOrDefaultAsync();
            return user == null ? null : MapToGetDto(user);
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

        public async Task<bool> UpdateCharacterAsync(Guid userId, UserCharacterUpdateDto characterDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return false;

            
            if (user.Character == null) user.Character = new UserCharacter();

            user.Character.Gender = characterDto.Gender;
            user.Character.Emotion = characterDto.Emotion;
            user.Character.Clothing = characterDto.Clothing;
            user.Character.HairColor = characterDto.HairColor;
            user.Character.Skin = characterDto.Skin;
            user.Character.ClothingColor = characterDto.ClothingColor;

            _context.Users.Update(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateUserAsync(Guid id, UserEditDTO dto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            user.Username = dto.Username;
            user.Name = dto.Name;
            user.Surname = dto.Surname;
            user.Email = dto.Email;
            user.Level = dto.Level;
            user.BirthDate = dto.BirthDate.ToUniversalTime();


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
            Username = u.Username,
            Name = u.Name,
            Surname = u.Surname,
            Level = u.Level,
            Email = u.Email,
            Character = u.Character,
            BirthDate = u.BirthDate,
            IsActive = u.IsActive,
            CreatedAt = u.CreatedAt
        };
    }
}