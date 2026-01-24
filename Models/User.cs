using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeDungeonAPI.Models
{
    [Table("users")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("age")]
        public int  Age { get; set; }

        // BU SƏTRİ ƏLAVƏ ET:
        // UserInfo ilə One-to-One əlaqəsini təmin edir
        public virtual UserInfo UserInfo { get; set; }
    }
}