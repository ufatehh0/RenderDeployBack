using CodeDungeon.Data;
using CodeDungeon.DTOs;
using CodeDungeon.DTOs.UserDTOs;
using CodeDungeon.Enums;
using CodeDungeon.Helpers;
using CodeDungeon.Models.Entities.User;
using CodeDungeon.Services.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CodeDungeon.Services.Concrete
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        public async Task<bool> RegisterAsync(UserCreateDto registerDto)
        {
            
            var exists = await _context.Users.AnyAsync(u => u.Email == registerDto.Email || u.Username == registerDto.Username);
            if (exists) return false;

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = registerDto.Username,
                Name = registerDto.Name,
                Surname = registerDto.Surname,
                Email = registerDto.Email,
                BirthDate = registerDto.BirthDate.ToUniversalTime(),
                Role = UserRole.User, 
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                PasswordHash = PasswordHasher.HashPassword(registerDto.Password),
                Character = new UserCharacter() 
            };

            await _context.Users.AddAsync(user);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<TokenResponseDto?> LoginAsync(UserLoginDto loginDto)
        {
            
            var user = await _context.Users.FirstOrDefaultAsync(u =>
                u.Username == loginDto.UsernameOrEmail || u.Email == loginDto.UsernameOrEmail);

            
            if (user == null || string.IsNullOrEmpty(user.PasswordHash))
            {
                return null;
            }

            
            bool isPasswordValid = PasswordHasher.VerifyPassword(loginDto.Password, user.PasswordHash);

            if (!isPasswordValid)
            {
                return null;
            }

            
            return await GenerateTokensAndUpdateUser(user);
        }



        public async Task<TokenResponseDto?> RefreshTokenAsync(string refreshToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);

            if (user == null || user.RefreshTokenExpiryTime <= DateTime.Now)
                return null;

            return await GenerateTokensAndUpdateUser(user);
        }

        public async Task<bool> LogoutAsync(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;
            return await _context.SaveChangesAsync() > 0;
        }

        private async Task<TokenResponseDto> GenerateTokensAndUpdateUser(User user)
        {
            var (accessToken, expiration) = GenerateJwtToken(user);
            var refreshToken = GenerateSecureRefreshToken();

            user.RefreshToken = refreshToken;
            
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _context.SaveChangesAsync(); 

            return new TokenResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpiration = expiration
            };
        }

        private (string token, DateTime expiration) GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            
            var expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["Jwt:DurationInMinutes"] ?? "15"));

            var claims = new[]
            {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.Role, user.Role.ToString()),
        
       
    };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: expires, 
                signingCredentials: credentials);

            return (new JwtSecurityTokenHandler().WriteToken(token), expires);
        }

        private string GenerateSecureRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            
            return Convert.ToBase64String(randomNumber);
        }




        /// Bu hisse tam yazilmayib sonra deyisecey!!!!
        public async Task<string?> ForgotPasswordAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

           
            if (user == null) return null;

           
            var resetToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));

            user.PasswordResetToken = resetToken;
            user.ResetTokenExpires = DateTime.UtcNow.AddHours(1); 

            await _context.SaveChangesAsync();

         
            return resetToken;
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u =>
                u.PasswordResetToken == dto.Token && u.ResetTokenExpires > DateTime.UtcNow);

            if (user == null) return false;

            user.PasswordHash = PasswordHasher.HashPassword(dto.NewPassword);
            user.PasswordResetToken = null; 
            user.ResetTokenExpires = null;

            return await _context.SaveChangesAsync() > 0;
        }









    }




    }