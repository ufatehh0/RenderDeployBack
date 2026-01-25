using Microsoft.AspNetCore.Mvc;
using CodeDungeonAPI.Data;
using CodeDungeonAPI.Models;
using CodeDungeonAPI.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace CodeDungeonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto request)
        {
            if (await _context.Users.AnyAsync(u => u.Email.ToLower() == request.Email.ToLower()))
                return BadRequest(new { message = "Bu email artıq mövcuddur!" });

            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                Age = request.Age,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Statistikaların yaradılması
            var userInfo = new UserInfo
            {
                Id = user.Id,
                UserLevel = 1,
                UserScore = 0,
                CurrentGameLevel = 1,
                ProfilePictureUrl = ""
            };

            // Xarakter görünüşünün yaradılması
            var userCharacter = new UserCharacter
            {
                Id = user.Id,
                Gender = "male",
                Emotion = "neutral",
                Clothing = "tshirt",
                HairColor = "#b96321",
                Skin = "#ffdbac",
                ClothingColor = "#3b82f6"
            };

            _context.UsersInfo.Add(userInfo);
            _context.UserCharacters.Add(userCharacter);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Qeydiyyat uğurludur!", token = CreateToken(user) });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == request.Email.ToLower());

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
                return Unauthorized(new { message = "Email və ya şifrə yanlışdır!" });

            return Ok(new { message = "Giriş uğurludur!", token = CreateToken(user) });
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetProfile()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();

            int userId = int.Parse(userIdClaim.Value);

            var userProfile = await _context.Users
                .Where(u => u.Id == userId)
                .Select(u => new
                {
                    u.Id,
                    u.Name,
                    u.Email,
                    u.Age,
                    Stats = new
                    {
                        u.UserInfo.UserLevel,
                        u.UserInfo.UserScore,
                        u.UserInfo.CurrentGameLevel,
                        u.UserInfo.ProfilePictureUrl
                    },
                    Character = new
                    {
                        u.UserCharacter.Gender,
                        u.UserCharacter.Emotion,
                        u.UserCharacter.Clothing,
                        u.UserCharacter.HairColor,
                        u.UserCharacter.Skin,
                        u.UserCharacter.ClothingColor
                    }
                })
                .FirstOrDefaultAsync();

            if (userProfile == null) return NotFound(new { message = "İstifadəçi tapılmadı." });

            return Ok(userProfile);
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("bu_cox_gizli_ve_uzun_bir_key_olmalidir_123!"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(securityToken);
        }
    }
}