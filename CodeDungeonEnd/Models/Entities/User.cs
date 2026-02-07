using CodeDungeon.Enums;
using System.ComponentModel.DataAnnotations;

namespace CodeDungeon.Models.Entities
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public UserRole Role { get; set; }


        public string? PasswordHash { get; set; } 
        

        [Required, StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string Surname { get; set; } = string.Empty;


        [Required, StringLength(7, MinimumLength = 7)]
        public string FinCode { get; set; } = string.Empty;

        [Required, Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

       
        public DateTime BirthDate { get; set; }

       

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }

        public string? PasswordResetToken { get; set; }
        public DateTime? ResetTokenExpires { get; set; }
        public User() { }

       
        public User(string username, string name, string surname, string email)
        {
            Username = username ?? throw new ArgumentNullException(nameof(username));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Surname = surname ?? throw new ArgumentNullException(nameof(surname));
            Email = email ?? throw new ArgumentNullException(nameof(email));
        }
    }
}