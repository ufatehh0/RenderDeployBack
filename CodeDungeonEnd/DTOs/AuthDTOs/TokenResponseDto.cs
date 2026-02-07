namespace CodeDungeon.DTOs
{

    public record RefreshTokenRequestDto(string RefreshToken);

    public record ForgotPasswordDto(string Email);
    public record ResetPasswordDto(string Token, string NewPassword);
    public class TokenResponseDto
    {
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
        
        public DateTime AccessTokenExpiration { get; set; }
    }
}