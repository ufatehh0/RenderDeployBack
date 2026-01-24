using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeDungeonAPI.Models
{
    [Table("users_info")]
    public class UserInfo
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Column("user_level")]
        public int UserLevel { get; set; } = 1; // Default olaraq 1-ci səviyyədən başlayır

        [Column("user_score")]
        public int UserScore { get; set; } = 0; // Default olaraq 0 xal

        [Column("current_game_level")]
        public int CurrentGameLevel { get; set; } = 1;

        [Column("profile_picture_url")]
        public string? ProfilePictureUrl { get; set; }

        // Navigation Property - User ilə əlaqə
        [ForeignKey("Id")]
        public virtual User User { get; set; }
    }
}