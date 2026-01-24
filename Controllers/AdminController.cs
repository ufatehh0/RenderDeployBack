using CodeDungeonAPI.Data;
using CodeDungeonAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeDungeonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _context;
        // Sənin gizli admin şifrən (Bunu istəsən appsettings.json-dan da oxuya bilərsən)
        private const string AdminSecretKey = "Super_Gizli_Admin_Sifresi_2026";

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("all-users")]
        public async Task<IActionResult> GetAllUsersData([FromHeader(Name = "X-Admin-Key")] string adminKey)
        {
            // 1. Header-dən gələn key yoxlanılır
            if (adminKey != AdminSecretKey)
            {
                return Unauthorized(new { message = "Giriş qadağandır! Yanlış Admin Key." });
            }

            // 2. Bütün userləri öz statistikaları ilə birgə (Join) çəkirik
            var allUsers = await _context.Users
                .Include(u => u.UserInfo) // UserInfo cədvəlini də gətiririk
                .Select(u => new
                {
                    u.Id,
                    u.Name,
                    u.Email,
                    u.Age,
                    // Statistikalar UserInfo-dan gəlir
                    Stats = u.UserInfo != null ? new
                    {
                        u.UserInfo.UserLevel,
                        u.UserInfo.UserScore,
                        u.UserInfo.CurrentGameLevel,
                        u.UserInfo.ProfilePictureUrl
                    } : null
                })
                .ToListAsync();

            return Ok(allUsers);
        }
        [HttpPut("update-user/{id}")]
        public async Task<IActionResult> UpdateUserAsAdmin(int id, [FromBody] AdminUpdateDto request, [FromHeader(Name = "X-Admin-Key")] string adminKey)
        {
            // 1. Admin Key yoxlanışı
            if (adminKey != AdminSecretKey)
            {
                return Unauthorized(new { message = "Giriş qadağandır!" });
            }

            // 2. İstifadəçini və onun statistikalarını bazadan tapırıq
            var user = await _context.Users
                .Include(u => u.UserInfo)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound(new { message = "İstifadəçi tapılmadı." });
            }

            // 3. Əsas məlumatların yenilənməsi
            user.Name = request.Name;
            user.Email = request.Email;

            // 4. Statistikaların (UserInfo) yenilənməsi
            if (user.UserInfo != null)
            {
                user.UserInfo.UserLevel = request.UserLevel;
                user.UserInfo.UserScore = request.UserScore;
                user.UserInfo.CurrentGameLevel = request.CurrentGameLevel;
                user.UserInfo.ProfilePictureUrl = request.ProfilePictureUrl;
            }

            await _context.SaveChangesAsync();

            return Ok(new { message = $"ID: {id} olan istifadəçi məlumatları uğurla yeniləndi!" });
        }
    }
}