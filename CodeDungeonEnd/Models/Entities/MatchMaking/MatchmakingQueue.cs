namespace CodeDungeon.Models.Entities.MatchMaking
{
    public class MatchmakingQueue
    {
        public static List<Guid> WaitingPlayers { get; set; } = new List<Guid>();
    }


}
