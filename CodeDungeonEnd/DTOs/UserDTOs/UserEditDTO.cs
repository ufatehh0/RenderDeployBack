using CodeDungeon.Enums;

namespace CodeDungeon.DTOs
{
    public class UserEditDTO

    {
        

        public string Username { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
         
        public string Email { get; set; } = string.Empty;


        public int Level { get; set; }
        public DateTime BirthDate { get; set; }
       
        
       
    }
}