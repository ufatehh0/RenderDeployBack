namespace CodeDungeonAPI.DTOs
{
    public class AdminUpdateDto
    {
        // User məlumatları
        public string Name { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }

        // UserInfo (Statistika)
        public int UserLevel { get; set; }
        public int UserScore { get; set; }
        public int CurrentGameLevel { get; set; }
        public string? ProfilePictureUrl { get; set; }

        // UserCharacter (Görünüş)
        public string Gender { get; set; }
        public string Emotion { get; set; }
        public string Clothing { get; set; }
        public string HairColor { get; set; }
        public string Skin { get; set; }
        public string ClothingColor { get; set; }
    }
}