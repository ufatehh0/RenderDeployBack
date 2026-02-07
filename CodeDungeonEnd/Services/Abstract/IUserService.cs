using CodeDungeon.DTOs;
using CodeDungeon.DTOs.UserDTOs;

namespace CodeDungeon.Services.Abstract
{
    public interface IUserService
    {
        Task<List<UserGetDto>> GetAllUsersAsync();
        Task<UserGetDto?> GetUserByIdAsync(Guid id);
        Task<UserGetDto?> GetUserByUserNameAsync(string username);
        Task<bool> CreateUserAsync(UserCreateDto dto);
        Task<bool> UpdateUserAsync(Guid id, UserEditDTO dto);
        Task<bool> DeleteUserAsync(Guid id);
    }
}