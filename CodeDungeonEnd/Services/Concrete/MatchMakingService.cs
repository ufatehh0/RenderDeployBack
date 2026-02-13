using CodeDungeon.Models.Entities.MatchMaking;
using CodeDungeon.Services.Abstract;
using System.Collections.Concurrent;

namespace CodeDungeon.Services.Concrete
{
    public class MatchMakingService : IMatchMakingService
    {
        private static readonly List<Guid> _waitingPlayers = new List<Guid>();
        private static readonly List<Match> _activeMatches = new List<Match>();
        private static readonly object _lock = new object();

        public void JoinQueue(Guid userId)
        {
            lock (_lock)
            {
                // SADECE AKTİV OLANLARI YOXLA
                // Eğer oyuncu zaten sıradaysa VEYA durumu "Active" olan bir maçın içindeyse ekleme yapma
                bool isAlreadyInMatch = _activeMatches.Any(m =>
                    (m.Player1Id == userId || m.Player2Id == userId) && m.Status == MatchStatus.Active);

                if (_waitingPlayers.Contains(userId) || isAlreadyInMatch)
                    return;

                _waitingPlayers.Add(userId);
                CheckAndStartMatch();
            }
        }

        public void LeaveQueue(Guid userId)
        {
            lock (_lock) { _waitingPlayers.Remove(userId); }
        }

        public void CheckAndStartMatch()
        {
            // Lock dışında çağrılma ihtimaline karşı tekrar lock (JoinQueue içinden geliyorsa gerekmez ama garanti olsun)
            lock (_lock)
            {
                while (_waitingPlayers.Count >= 2)
                {
                    var p1 = _waitingPlayers[0];
                    var p2 = _waitingPlayers[1];

                    var match = new Match { Player1Id = p1, Player2Id = p2 };
                    _activeMatches.Add(match);
                    _waitingPlayers.RemoveRange(0, 2);
                }
            }
        }

        public void LeaveMatch(Guid userId)
        {
            lock (_lock)
            {
                // Oyuncunun içinde olduğu aktif maçı bul
                var match = _activeMatches.FirstOrDefault(m =>
                    (m.Player1Id == userId || m.Player2Id == userId) && m.Status == MatchStatus.Active);

                if (match != null)
                {
                    // Maçtan çıkan kişi kaybeder, diğeri kazanır (Hükmen)
                    match.Status = MatchStatus.Cancelled;
                    match.EndedAt = DateTime.UtcNow;
                    match.WinnerId = (match.Player1Id == userId) ? match.Player2Id : match.Player1Id;

                    // İsteğe bağlı: _activeMatches içinden silebilir veya "Arşiv" listesine taşıyabilirsin
                }
            }
        }

        public void EndMatch(Guid matchId)
        {
            lock (_lock)
            {
                var match = _activeMatches.FirstOrDefault(m => m.Id == matchId);
                if (match != null)
                {
                    match.Status = MatchStatus.Finished;
                    match.EndedAt = DateTime.UtcNow;
                    _activeMatches.Remove(match); // Aktif listesinden temizle
                }
            }
        }

        public List<Guid> GetWaitingPlayers()
        {
            lock (_lock) { return _waitingPlayers.ToList(); }
        }

        public List<Match> GetActiveMatches()
        {
            lock (_lock) { return _activeMatches.ToList(); }
        }

       
    }
}