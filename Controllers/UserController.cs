using Microsoft.AspNetCore.Mvc;
using CodeDungeonAPI.Data;
using CodeDungeonAPI.Models;
using CodeDungeonAPI.DTOs; // Yeni DTO-lar üçün
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Qeydiyyat uğurludur!", token = CreateToken(user) });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == request.Email.ToLower());

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return Unauthorized(new { message = "Email və ya şifrə yanlışdır!" });
            }

            return Ok(new { message = "Giriş uğurludur!", token = CreateToken(user) });
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Name, user.Name)
    };

            // SHA256 üçün bu key kifayətdir (ən az 32 simvol)
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("bu_cox_gizli_ve_uzun_bir_key_olmalidir_123!"));

            // BURANI DƏYİŞDİK: HmacSha512Signature -> HmacSha256Signature
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(securityToken);
        }
    }
}