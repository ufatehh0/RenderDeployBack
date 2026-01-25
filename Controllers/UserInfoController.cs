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
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        var userInfo = await _context.UsersInfo.FirstOrDefaultAsync(ui => ui.Id == userId);

        if (userInfo == null) return NotFound();

        userInfo.UserLevel = request.UserLevel;
        userInfo.UserScore = request.UserScore;
        userInfo.CurrentGameLevel = request.CurrentGameLevel;
        userInfo.ProfilePictureUrl = request.ProfilePictureUrl;

        await _context.SaveChangesAsync();
        return Ok(new { message = "Statistikalar yeniləndi!" });
    }

    [HttpPut("update-character")]
    public async Task<IActionResult> UpdateCharacter([FromBody] CharacterUpdateDto request)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        var character = await _context.UserCharacters.FirstOrDefaultAsync(uc => uc.Id == userId);

        if (character == null) return NotFound();

        character.Gender = request.Gender;
        character.Emotion = request.Emotion;
        character.Clothing = request.Clothing;
        character.HairColor = request.HairColor;
        character.Skin = request.Skin;
        character.ClothingColor = request.ClothingColor;

        await _context.SaveChangesAsync();
        return Ok(new { message = "Xarakter görünüşü yeniləndi!" });
    }
}