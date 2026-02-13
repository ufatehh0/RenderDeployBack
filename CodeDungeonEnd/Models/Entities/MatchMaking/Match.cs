namespace CodeDungeon.Models.Entities.MatchMaking
{
    public enum MatchStatus { Active, Finished, Cancelled }

    public class Match
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid Player1Id { get; set; }
        public Guid Player2Id { get; set; }
        public DateTime StartedAt { get; set; } = DateTime.UtcNow;
        public DateTime? EndedAt { get; set; }
        public MatchStatus Status { get; set; } = MatchStatus.Active;
        public Guid? WinnerId { get; set; }
    }
}