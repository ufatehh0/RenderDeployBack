using CodeDungeon.DTOs;
using CodeDungeon.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CodeDungeonEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto loginDto)
        {
            var result = await _authService.LoginAsync(loginDto);
            if (result == null)
                return Unauthorized(new { message = "İstifadəçi adı və ya şifrə yanlışdır." });

            return Ok(result);
        }

        [Authorize]
        [HttpPost("set-initial-password")]
        public async Task<IActionResult> SetPassword([FromBody] SetInitialPasswordDto dto)
        {
            
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdStr))
                return Unauthorized();

            var userId = Guid.Parse(userIdStr);

           
            var result = await _authService.SetInitialPasswordAsync(userId, dto.NewPassword);

            if (result == null)
                return BadRequest(new { message = "Xəta baş verdi və ya şifrə artıq təyin edilib." });

            return Ok(new { message = "Şifrə uğurla təyin edildi.", data = result });
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequestDto dto)
        {
            var result = await _authService.RefreshTokenAsync(dto.RefreshToken);
            if (result == null)
                return Unauthorized(new { message = "Refresh token keçərsizdir." });

            return Ok(result);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr)) return BadRequest();

            var result = await _authService.LogoutAsync(Guid.Parse(userIdStr));
            return result ? Ok(new { message = "Çıxış edildi." }) : BadRequest();
        }


        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            var resetToken = await _authService.ForgotPasswordAsync(dto.Email);

            if (resetToken == null)
                return BadRequest(new { message = "İstifadəçi tapılmadı." });

           
            return Ok(new
            {
                message = "Şifrə sıfırlama tokeni yaradıldı.",
                resetToken = resetToken
            });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var result = await _authService.ResetPasswordAsync(dto);
            if (!result) return BadRequest("Link keçərsizdir və ya vaxtı bitib.");

            return Ok(new { message = "Şifrə uğurla yeniləndi." });
        }
    }
}