using CodeDungeonAPI.Data;
using CodeDungeonAPI.DTOs;
using CodeDungeonAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserInfoController : ControllerBase
{
    private readonly AppDbContext _context;
    public UserInfoController(AppDbContext context) { _context = context; }

    
    [HttpPut("update-stats")]
    public async Task<IActionResult> UpdateStats([FromBody] UserInfoUpdateDto request)
    {
        // 1. İstifadəçi ID-sini tokendən götürürük
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null) return Unauthorized();

        int userId = int.Parse(userIdClaim.Value);

        // 2. Bazadan mövcud məlumatı tapırıq
        var userInfo = await _context.UsersInfo.FirstOrDefaultAsync(ui => ui.Id == userId);
        if (userInfo == null) return NotFound(new { message = "İstifadəçi statistikası tapılmadı." });

        // 3. YALNIZ gələn (null olmayan) sahələri yeniləyirik
        // Beləliklə, JSON-da göndərmədiyin sahə bazada köhnə rəqəmi ilə qalacaq
        if (request.UserLevel.HasValue)
            userInfo.UserLevel = request.UserLevel.Value;

        if (request.UserScore.HasValue)
            userInfo.UserScore = request.UserScore.Value;

        if (request.CurrentGameLevel.HasValue)
            userInfo.CurrentGameLevel = request.CurrentGameLevel.Value;

        if (!string.IsNullOrEmpty(request.ProfilePictureUrl))
            userInfo.ProfilePictureUrl = request.ProfilePictureUrl;

        // 4. Dəyişiklikləri yadda saxlayırıq
        await _context.SaveChangesAsync();

        return Ok(new { message = "Məlumatlar uğurla yeniləndi!" });
    }

    [HttpPut("update-character")]
    public async Task<IActionResult> UpdateCharacter([FromBody] CharacterUpdateDto request)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null) return Unauthorized();

        var userId = int.Parse(userIdClaim.Value);
        var character = await _context.UserCharacters.FirstOrDefaultAsync(uc => uc.Id == userId);

        if (character == null) return NotFound(new { message = "Xarakter tapılmadı." });

        // Yalnız gələn (boş olmayan) sahələri yeniləyirik
        if (!string.IsNullOrEmpty(request.Gender))
            character.Gender = request.Gender;

        if (!string.IsNullOrEmpty(request.Emotion))
            character.Emotion = request.Emotion;

        if (!string.IsNullOrEmpty(request.Clothing))
            character.Clothing = request.Clothing;

        if (!string.IsNullOrEmpty(request.HairColor))
            character.HairColor = request.HairColor;

        if (!string.IsNullOrEmpty(request.Skin))
            character.Skin = request.Skin;

        if (!string.IsNullOrEmpty(request.ClothingColor))
            character.ClothingColor = request.ClothingColor;

        await _context.SaveChangesAsync();
        return Ok(new { message = "Xarakter görünüşü uğurla yeniləndi!" });
    }
}