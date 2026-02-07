using CodeDungeon.DTOs;

namespace CodeDungeon.Services.Abstract
{
    public interface IAuthService
    {
        Task<TokenResponseDto?> LoginAsync(UserLoginDto loginDto);
       
        Task<TokenResponseDto?> RefreshTokenAsync(string refreshToken);
        Task<bool> LogoutAsync(Guid userId);


        Task<string?> ForgotPasswordAsync(string email);
        Task<bool> ResetPasswordAsync(ResetPasswordDto dto);
    }
}