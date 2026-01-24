namespace CodeDungeonAPI.DTOs
{
    public class AdminUpdateDto
    {
        // User cədvəli üçün
        public string Name { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }

        // UserInfo cədvəli üçün
        public int UserLevel { get; set; }
        public int UserScore { get; set; }
        public int CurrentGameLevel { get; set; }
        public string? ProfilePictureUrl { get; set; }
    }
}