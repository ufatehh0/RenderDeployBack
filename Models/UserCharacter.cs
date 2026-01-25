using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CodeDungeonAPI.Models
{
    public class UserCharacter
    {
        [Key]
        [ForeignKey("User")]
        public int Id { get; set; }

        public string Gender { get; set; } = "male";
        public string Emotion { get; set; } = "neutral";
        public string Clothing { get; set; } = "tshirt";
        public string HairColor { get; set; } = "#b96321";
        public string Skin { get; set; } = "#ffdbac";
        public string ClothingColor { get; set; } = "#3b82f6";

        [JsonIgnore] // Döngüsel referansı önlemek için
        public virtual User User { get; set; }
    }
}