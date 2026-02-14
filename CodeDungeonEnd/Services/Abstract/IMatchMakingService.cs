using CodeDungeon.Models.Entities.MatchMaking;

namespace CodeDungeon.Services.Abstract
{
    public interface IMatchMakingService
    {
        void JoinQueue(Guid userId);
        void LeaveQueue(Guid userId);
        List<Guid> GetWaitingPlayers();
        List<Match> GetActiveMatches();
        void CheckAndStartMatch();
        void LeaveMatch(Guid userId);
        void EndMatch(Guid matchId);

        void AdminRemoveFromQueue(Guid userId);
    }
}