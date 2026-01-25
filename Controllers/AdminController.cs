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
        if (adminKey != AdminSecretKey) return Unauthorized();

        var user = await _context.Users
            .Include(u => u.UserInfo)
            .Include(u => u.UserCharacter)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null) return NotFound();

        user.Name = request.Name;
        user.Email = request.Email;

        if (user.UserInfo != null)
        {
            user.UserInfo.UserLevel = request.UserLevel;
            user.UserInfo.UserScore = request.UserScore;
            user.UserInfo.CurrentGameLevel = request.CurrentGameLevel;
        }

        // Əgər DTO-da xarakter sahələri varsa bura əlavə edə bilərsən
        await _context.SaveChangesAsync();
        return Ok(new { message = "Yeniləndi!" });
    }
}