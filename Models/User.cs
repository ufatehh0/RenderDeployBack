using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeDungeonAPI.Models
{
    public class User
    {
        [Key] // Bu sütunun Primary Key olduğunu bildirir
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // ID-nin baza tərəfindən yaradıldığını bildirir
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("password")]
        public string Password { get; set; }
    }
}