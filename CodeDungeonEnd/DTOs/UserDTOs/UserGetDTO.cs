using CodeDungeon.Enums;
using CodeDungeon.Models.Entities.User;

namespace CodeDungeon.DTOs
{
    public class UserGetDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public UserCharacter Character { get; set; } = new UserCharacter();
        public DateTime BirthDate { get; set; }
      
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
       
    }
}