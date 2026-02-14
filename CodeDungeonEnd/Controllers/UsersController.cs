using CodeDungeon.DTOs;
using CodeDungeon.DTOs.UserDTOs;
using CodeDungeon.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CodeDungeonEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);

            
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized("Token etibarsızdır.");

            
            var user = await _userService.GetUserByIdAsync(Guid.Parse(userIdStr));

            if (user == null) return NotFound("İstifadəçi tapılmadı.");
                
            return Ok(user);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound("İstifadəçi tapılmadı.");
            return Ok(user);
        }

        [HttpGet("username/{username}")]
        public async Task<IActionResult> GetByUsername(string username)
        {
            var user = await _userService.GetUserByUserNameAsync(username);
            if (user == null) return NotFound("İstifadəçi tapılmadı.");
            return Ok(user);
        }

        [HttpPut("update-character")]
        [Authorize(Roles = "User,SuperAdmin")]
        public async Task<IActionResult> UpdateCharacter([FromBody] UserCharacterUpdateDto dto)
        {
            
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized();

            var result = await _userService.UpdateCharacterAsync(Guid.Parse(userIdStr), dto);

            if (!result) return BadRequest(new { message = "Xarakter yenilənərkən xəta baş verdi." });

            return Ok(new { message = "Xarakter uğurla yeniləndi." });
        }

        [HttpPut("edituser/{id:guid}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UserEditDTO dto)
        {
            var result = await _userService.UpdateUserAsync(id, dto);
            if (!result) return BadRequest("İstifadəçi tapılmadı veya güncelleme başarısız.");

            return Ok("İstifadəçi məlumatları uğurla yeniləndi.");
        }

        [HttpDelete("delete/{id:guid}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (result) return Ok("İstifadəçi silindi.");
            return NotFound("Silinəcək istifadəçi tapılmadı.");
        }
    }
}