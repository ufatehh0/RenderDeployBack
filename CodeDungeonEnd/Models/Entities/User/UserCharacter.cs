using CodeDungeon.Enums;
using System.ComponentModel.DataAnnotations;

namespace CodeDungeon.Models.Entities.User
{
    public class UserCharacter
    {
        public string Gender { get; set; } = string.Empty;
        public string Emotion { get; set; } = string.Empty;
        public string Clothing { get; set; } = string.Empty;
        public string HairColor { get; set; } = string.Empty;
        public string Skin { get; set; } = string.Empty;
        public string ClothingColor { get; set; } = string.Empty;

    }
}
