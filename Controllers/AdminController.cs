using CodeDungeonAPI.Data;
using CodeDungeonAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


[Route("api/[controller]")]
[ApiController]
public class AdminController : ControllerBase
{
    private readonly AppDbContext _context;
    private const string AdminSecretKey = "Super_Gizli_Admin_Sifresi_2026";

    public AdminController(AppDbContext context) { _context = context; }

    [HttpGet("all-users")]
    public async Task<IActionResult> GetAllUsersData([FromHeader(Name = "X-Admin-Key")] string adminKey)
    {
        if (adminKey != AdminSecretKey) return Unauthorized(new { message = "Yanlış Admin Key." });

        var allUsers = await _context.Users
            .Select(u => new
            {
                u.Id,
                u.Name,
                u.Email,
                u.Age,
                Stats = u.UserInfo,
                Character = u.UserCharacter
            })
            .ToListAsync();

        return Ok(allUsers);
    }

    [HttpPut("update-user/{id}")]
    public async Task<IActionResult> UpdateUserAsAdmin(int id, [FromBody] AdminUpdateDto request, [FromHeader(Name = "X-Admin-Key")] string adminKey)
    {
        // 1. Admin Key yoxlanışı
        if (adminKey != AdminSecretKey) return Unauthorized(new { message = "Giriş qadağandır!" });

        // 2. İstifadəçini bütün əlaqəli cədvəllərlə birlikdə tapırıq
        var user = await _context.Users
            .Include(u => u.UserInfo)
            .Include(u => u.UserCharacter)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null) return NotFound(new { message = "İstifadəçi tapılmadı." });

        // 3. User cədvəli üçün (Name, Email, Age)
        if (!string.IsNullOrEmpty(request.Name)) user.Name = request.Name;
        if (!string.IsNullOrEmpty(request.Email)) user.Email = request.Email;
        if (request.Age > 0) user.Age = (int)request.Age;

        // 4. UserInfo (Stats) yeniləmə - Yalnız gələn dəyərlər
        if (user.UserInfo != null)
        {
            if (request.UserLevel.HasValue) user.UserInfo.UserLevel = request.UserLevel.Value;
            if (request.UserScore.HasValue) user.UserInfo.UserScore = request.UserScore.Value;
            if (request.CurrentGameLevel.HasValue) user.UserInfo.CurrentGameLevel = request.CurrentGameLevel.Value;
            if (!string.IsNullOrEmpty(request.ProfilePictureUrl)) user.UserInfo.ProfilePictureUrl = request.ProfilePictureUrl;
        }

        // 5. UserCharacter (Görünüş) yeniləmə - Yalnız gələn dəyərlər
        if (user.UserCharacter != null)
        {
            if (!string.IsNullOrEmpty(request.Gender)) user.UserCharacter.Gender = request.Gender;
            if (!string.IsNullOrEmpty(request.Emotion)) user.UserCharacter.Emotion = request.Emotion;
            if (!string.IsNullOrEmpty(request.Clothing)) user.UserCharacter.Clothing = request.Clothing;
            if (!string.IsNullOrEmpty(request.HairColor)) user.UserCharacter.HairColor = request.HairColor;
            if (!string.IsNullOrEmpty(request.Skin)) user.UserCharacter.Skin = request.Skin;
            if (!string.IsNullOrEmpty(request.ClothingColor)) user.UserCharacter.ClothingColor = request.ClothingColor;
        }

        await _context.SaveChangesAsync();
        return Ok(new { message = $"ID: {id} olan istifadəçinin gələn məlumatları uğurla yeniləndi!" });
    }

    [HttpDelete("delete-user/{id}")]
    public async Task<IActionResult> DeleteUserAsAdmin(int id, [FromHeader(Name = "X-Admin-Key")] string adminKey)
    {
        // 1. Admin Key yoxlanışı
        if (adminKey != AdminSecretKey)
            return Unauthorized(new { message = "Giriş qadağandır! Yanlış Admin Key." });

        // 2. İstifadəçini tapırıq
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
            return NotFound(new { message = "İstifadəçi tapılmadı." });

        // 3. İstifadəçini silirik
        // Qeyd: Bazada "ON DELETE CASCADE" quraşdırdığımız üçün UserInfo və UserCharacter avtomatik silinəcək.
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return Ok(new { message = $"ID: {id} olan istifadəçi və ona aid bütün məlumatlar uğurla silindi!" });
    }
}