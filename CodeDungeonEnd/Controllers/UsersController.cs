using CodeDungeon.DTOs;
using CodeDungeon.DTOs.UserDTOs;
using CodeDungeon.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
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

        [HttpPost("create")]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> Create([FromBody] UserCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _userService.CreateUserAsync(dto);
            if (result) return Ok("İstifadəçi uğurla yaradıldı. İlk giriş üçün Fin Kod istifadə edilə bilər.");

            return BadRequest("İstifadəçi yaradıla bilmədi (Email və ya Username artıq mövcud ola bilər).");
        }

        [HttpPut("edit/{id:guid}")]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UserEditDTO dto)
        {
            var result = await _userService.UpdateUserAsync(id, dto);
            if (!result) return BadRequest("İstifadəçi tapılmadı veya güncelleme başarısız.");

            return Ok("İstifadəçi məlumatları uğurla yeniləndi.");
        }

        [HttpDelete("delete/{id:guid}")]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (result) return Ok("İstifadəçi silindi.");
            return NotFound("Silinəcək istifadəçi tapılmadı.");
        }
    }
}