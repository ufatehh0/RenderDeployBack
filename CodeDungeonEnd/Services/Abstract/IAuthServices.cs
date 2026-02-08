using CodeDungeon.DTOs;
using CodeDungeon.DTOs.UserDTOs;

namespace CodeDungeon.Services.Abstract
{
    public interface IAuthService
    {


        Task<TokenResponseDto?> RegisterAsync(UserCreateDto registerDto);
        Task<TokenResponseDto?> LoginAsync(UserLoginDto loginDto);
       
        Task<TokenResponseDto?> RefreshTokenAsync(string refreshToken);
        Task<bool> LogoutAsync(Guid userId);


        Task<string?> ForgotPasswordAsync(string email);
        Task<bool> ResetPasswordAsync(ResetPasswordDto dto);
    }
}