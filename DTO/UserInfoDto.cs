namespace CodeDungeonAPI.DTOs
{
    public class UserInfoUpdateDto
    {
        public int UserLevel { get; set; }
        public int UserScore { get; set; }
        public int CurrentGameLevel { get; set; }
        public string? ProfilePictureUrl { get; set; }
    }
}