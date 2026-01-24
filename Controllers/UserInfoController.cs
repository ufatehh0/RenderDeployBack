using Microsoft.AspNetCore.Mvc;
using CodeDungeonAPI.Data;
using CodeDungeonAPI.Models;
using CodeDungeonAPI.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CodeDungeonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Bütün metodlar üçün JWT token tələb olunur
    public class UserInfoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserInfoController(AppDbContext context)
        {
            _context = context;
        }

        // Oyunçunun öz statistikasını yeniləməsi üçün POST və ya PUT metodu
        [HttpPost("update")]
        public async Task<IActionResult> UpdateStats([FromBody] UserInfoUpdateDto request)
        {
            // 1. Token daxilindən istifadəçinin ID-sini tapırıq
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();

            int userId = int.Parse(userIdClaim.Value);

            // 2. Bazadan həmin istifadəçiyə aid UserInfo-nu tapırıq
            var userInfo = await _context.UsersInfo.FirstOrDefaultAsync(ui => ui.Id == userId);

            if (userInfo == null)
            {
                return NotFound(new { message = "İstifadəçi statistikası tapılmadı." });
            }

            // 3. Məlumatları yeniləyirik
            userInfo.UserLevel = request.UserLevel;
            userInfo.UserScore = request.UserScore;
            userInfo.CurrentGameLevel = request.CurrentGameLevel;
            userInfo.ProfilePictureUrl = request.ProfilePictureUrl;

            // 4. Dəyişiklikləri yadda saxlayırıq
            await _context.SaveChangesAsync();

            return Ok(new { message = "Statistikalar uğurla yeniləndi!" });
        }
    }
}