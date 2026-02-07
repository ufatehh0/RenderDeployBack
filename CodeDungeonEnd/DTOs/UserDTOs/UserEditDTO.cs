using CodeDungeon.Enums;

namespace CodeDungeon.DTOs
{
    public class UserEditDTO

    {
        public UserRole Role { get; set; }

        public string Username { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string FatherName { get; set; } = string.Empty;
        
        public string FinCode { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
       
        
       
    }
}