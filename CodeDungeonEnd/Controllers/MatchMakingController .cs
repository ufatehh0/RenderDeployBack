using CodeDungeon.Enums; // MatchStatus Enum-un olduğu yer
using CodeDungeon.Models.Entities.MatchMaking;
using CodeDungeon.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CodeDungeonEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MatchmakingController : ControllerBase
    {
        private readonly IMatchMakingService _matchmakingService;

        public MatchmakingController(IMatchMakingService matchmakingService)
        {
            _matchmakingService = matchmakingService;
        }

        [HttpPost("join")]
        public IActionResult Join()
        {
            var userId = GetCurrentUserId();

            // Servis içindeki listeyi kontrol et (Daha güvenli)
            var waitingPlayers = _matchmakingService.GetWaitingPlayers();
            if (waitingPlayers.Contains(userId))
                return BadRequest(new { message = "Artıq növbədəsiniz." });

            var activeMatches = _matchmakingService.GetActiveMatches();
            var hasActiveMatch = activeMatches.Any(m =>
                (m.Player1Id == userId || m.Player2Id == userId) && m.Status == MatchStatus.Active);

            if (hasActiveMatch)
                return BadRequest(new { message = "Davam edən aktiv oyununuz var." });

            // Her şey tamamsa sıraya ekle
            _matchmakingService.JoinQueue(userId);

            //// Eklenip eklenmediğini son bir kez teyit et (Debugging için)
            //if (!_matchmakingService.GetWaitingPlayers().Contains(userId))
            //    return StatusCode(500, new { message = "Növbəyə daxil olarkən xəta baş verdi." });

            return Ok(new { message = "Növbəyə girildi." });
        }

        // 2. ÖZ AKTİV OYUNUNA BAXMA
        [HttpGet("my-match")]
        public IActionResult GetMyMatch()
        {
            var userId = GetCurrentUserId();

            // Yalnız statusu Active olanı gətir
            var match = _matchmakingService.GetActiveMatches()
                .FirstOrDefault(m => (m.Player1Id == userId || m.Player2Id == userId)
                                     && m.Status == MatchStatus.Active);

            if (match == null)
                return NotFound(new { message = "Hazırda aktiv oyununuz yoxdur." });

            return Ok(match);
        }

        // 3. OYUNDAN AYRILMA (Məğlub sayılaraq)
        [HttpPost("leave-match")]
        public IActionResult LeaveMatch()
        {
            var userId = GetCurrentUserId();

            // Yalnız Active olan oyundan çıxmaq olar
            var activeMatch = _matchmakingService.GetActiveMatches()
                .FirstOrDefault(m => (m.Player1Id == userId || m.Player2Id == userId)
                                     && m.Status == MatchStatus.Active);

            if (activeMatch == null)
                return BadRequest(new { message = "Tərk ediləcək aktiv bir oyun tapılmadı." });

            _matchmakingService.LeaveMatch(userId);
            return Ok(new { message = "Maçdan ayrıldınız, hükmen mağlup sayıldınız." });
        }

        // 4. NÖVBƏDƏN ÇIXMA
        [HttpDelete("leave-queue")]
        public IActionResult LeaveQueue()
        {
            var userId = GetCurrentUserId();

            if (!_matchmakingService.GetWaitingPlayers().Contains(userId))
                return BadRequest(new { message = "Sırada deyilsiniz, çıxa bilməzsiniz." });

            _matchmakingService.LeaveQueue(userId);
            return Ok(new { message = "Növbədən çıxıldı." });
        }

        //// 5. OYUN TARİXÇƏSİ (İstifadəçinin keçmiş oyunları)
        //[HttpGet("history")]
        //public async Task<IActionResult> GetHistory()
        //{
        //    // Servisində bu metodun olduğunu fərz edirik
        //    var history = await _matchmakingService.GetMatchHistoryAsync(GetCurrentUserId());
        //    return Ok(history);
        //}

        // 6. ADMİN: BÜTÜN AKTİV OYUNLAR
        [HttpGet("admin/active-matches")]
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult GetAllActiveMatches()
        {
            // Adminlər bütün matçları (Active, Finished, Cancelled) görə bilsin deyə filter qoymuruq
            return Ok(_matchmakingService.GetActiveMatches());
        }

        // 7. ADMİN: NÖVBƏDƏKİ HER KƏS
        [HttpGet("admin/queue")]
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult GetQueue()
        {
            var players = _matchmakingService.GetWaitingPlayers();
            return Ok(new { count = players.Count, userIds = players });
        }

        // 8. ADMİN: OYUNU SONLANDIRMA
        [HttpPost("admin/end/{matchId:guid}")]
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult EndMatch(Guid matchId)
        {
            _matchmakingService.EndMatch(matchId);
            return Ok(new { message = "Maç admin tərəfindən sonlandırıldı." });
        }

        private Guid GetCurrentUserId()
        {
            var claim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return claim != null ? Guid.Parse(claim) : Guid.Empty;
        }
    }
}